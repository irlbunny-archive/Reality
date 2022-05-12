using Reality.ModLoader.Hooking;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Plugins;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using Reality.ModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Reality.ModLoader
{
    public static class Loader
    {
        public static bool IsInitialized { get; private set; }

        public static List<GamePlugin> LoadedPlugins { get; } = new();

        public static IMemory Memory { get; private set; }
        public static ObjectStore Objects { get; private set; }

        public static void ProcessEventInternalHook(IntPtr thisPtr, IntPtr func, IntPtr parms)
        {
            var processEvent = true;
            if (thisPtr != IntPtr.Zero && func != IntPtr.Zero)
            {
                var thisObj = (UObject) thisPtr;
                var funcObj = (UObject) func;

                foreach (var plugin in LoadedPlugins)
                {
                    if (!plugin.OnProcessEvent(thisObj, funcObj, parms))
                    {
                        processEvent = false;
                        break;
                    }
                }
            }

            if (processEvent)
                UObject.ProcessEventInternal(thisPtr, func, parms);
        }

        internal static void Initialize()
        {
            if (IsInitialized)
                return;

            Memory = new InternalMemory();

            LoadPlugins();

            var objectsOffset = MemoryUtility.FindPattern(
                "\x48\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x8B\xD6",
                "xxx????x????x????x????xxx"
            );
            Objects = new FixedObjectStore(MemoryUtility.GetAddressFromOffset(objectsOffset, 7, 3));

            MinHook.CreateHook(Marshal.GetFunctionPointerForDelegate(UObject.ProcessEventInternal), ProcessEventInternalHook, out UObject.ProcessEventInternal);
            MinHook.EnableAllHooks();

            IsInitialized = true;
        }

        private static void LoadPlugins()
        {
            var fileNames = Directory.GetFiles(Bootstrap.PluginsPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var fileName in fileNames)
            {
                Logger.Info($"Loading \"{fileName}\"...");

                var types = Assembly.LoadFrom(fileName).GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(GamePlugin)));
                foreach (var type in types)
                {
                    var plugin = (BasePlugin) Activator.CreateInstance(type);
                    if (plugin is GamePlugin gamePlugin)
                        LoadedPlugins.Add(gamePlugin);

                    Logger.Info($"Loaded plugin: \"{plugin.Name}\" (v{plugin.Version}) by \"{plugin.Author}\"");
                }
            }
        }

        /// <summary>
        /// Checks if a game plugin is loaded by name.
        /// </summary>
        /// <param name="name">The game plugin name.</param>
        /// <returns>If it's loaded, true. Otherwise, false.</returns>
        public static bool IsPluginLoaded(string name)
            => LoadedPlugins.Any(x => x.Name == name);
    }
}
