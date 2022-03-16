using Reality.ModLoader.Memory;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using Reality.ModLoader.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Reality.ModLoader
{
    public class Loader
    {
        private static Loader _instance;
        public static Loader Instance => _instance ?? (_instance = new());

        public static string Version => $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";

        public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reality");
        public static string ResourcesPath => Path.Combine(DataPath, "Resources");
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");
        public static string LogsPath => Path.Combine(DataPath, "Logs");

        public IMemory Memory { get; private set; }
        public ObjectStore Objects { get; private set; }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProcessEventDelegate(IntPtr thisPtr, IntPtr func, IntPtr parms);
        public static ProcessEventDelegate ProcessEvent;

        private static void ProcessEventHook(IntPtr thisPtr, IntPtr func, IntPtr parms)
        {
            var processEvent = true;
            if (thisPtr != IntPtr.Zero && func != IntPtr.Zero)
            {
                var thisObj = (UObject) thisPtr;
                var funcObj = (UObject) func;

                foreach (var gamePlugin in PluginManager.GamePlugins)
                {
                    if (!gamePlugin.OnProcessEvent(thisObj, funcObj, parms))
                    {
                        processEvent = false;
                        break;
                    }
                }
            }

            if (processEvent)
                ProcessEvent(thisPtr, func, parms);
        }

        public void Initialize()
        {
            // Create all directories.
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(ResourcesPath);
            Directory.CreateDirectory(PluginsPath);
            Directory.CreateDirectory(LogsPath);

            Logger.FilePath = Path.Combine(LogsPath, $"{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt");
            Logger.Info($"Reality ({Version}), made with <3 by Kaitlyn.");
            Logger.Info($"Consider supporting me on Patreon! https://www.patreon.com/join/ItsKaitlyn03");

            Memory = new InternalMemory();

            PluginManager.LoadAll();

            Objects = new FixedObjectStore(PluginManager.LoaderPlugin.Grab<IntPtr>("OBJECT_STORE_ADDRESS"));

            Logger.Info("Applying ProcessEvent hook...");

            MinHook.CreateHook(PluginManager.LoaderPlugin.Grab<IntPtr>("PROCESSEVENT_ADDRESS"), ProcessEventHook, out ProcessEvent);
            MinHook.EnableAllHooks();
        }
    }
}
