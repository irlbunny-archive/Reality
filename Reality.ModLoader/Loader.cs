using Reality.ModLoader.Hooking;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Plugins;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using Reality.ModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader
{
    public static class Loader
    {
        public static bool Initialized { get; private set; }
        public static bool GameInitialized { get; private set; }

        public static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public static string DataPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reality");
        public static string ResourcesPath => Path.Combine(DataPath, "Resources");
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");
        public static string LogsPath => Path.Combine(DataPath, "Logs");

        public static IMemory Memory { get; private set; }
        public static ObjectStore Objects { get; private set; }

        public static List<UnrealPlugin> LoadedPlugins { get; } = new();

        public static void ProcessEventInternalHook(IntPtr thisPtr, IntPtr func, IntPtr parms)
        {
            if (!GameInitialized)
            {
                foreach (var plugin in LoadedPlugins)
                {
                    plugin.OnGameInitialized();
                }

                GameInitialized = true;
            }

            var result = true;
            if (thisPtr != IntPtr.Zero && func != IntPtr.Zero)
            {
                var thisObj = (UObject) thisPtr;
                var funcObj = (UObject) func;

                foreach (var plugin in LoadedPlugins)
                {
                    if (!plugin.OnProcessEvent(thisObj, funcObj, parms))
                    {
                        result = false;
                        break;
                    }
                }
            }

            if (result)
                UObject.ProcessEventInternal(thisPtr, func, parms);
        }

        public static void Initialize()
        {
            if (Initialized)
                return;

            Memory = new InternalMemory();

            LoadPlugins();

            Objects = new FixedObjectStore(Configuration.GetAddressFromName("GObjects"));

            MinHook.CreateHook(UObject.ProcessEventInternal, ProcessEventInternalHook, out UObject.ProcessEventInternal);
            MinHook.EnableAllHooks();

            Initialized = true;
        }

        private static void LoadPlugins()
        {
            var fileNames = Directory.GetFiles(PluginsPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var fileName in fileNames)
            {
                Logger.Info($"Loading \"{fileName}\"...");

                var types = Assembly.LoadFrom(fileName).GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(UnrealPlugin)));
                foreach (var type in types)
                {
                    var plugin = (BasePlugin) Activator.CreateInstance(type);
                    if (plugin is UnrealPlugin gamePlugin)
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
