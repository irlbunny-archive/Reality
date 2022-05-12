using Reality.ModLoader.Hooking;
using Reality.ModLoader.Memory;
using Reality.ModLoader.Plugins;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using Reality.ModLoader.Utilities;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Reality.ModLoader
{
    public class Loader
    {
        private static Loader _instance;
        public static Loader Instance => _instance ?? (_instance = new());

        public IMemory Memory { get; private set; }
        public ObjectStore Objects { get; private set; }

        public static void ProcessEventInternalHook(IntPtr thisPtr, IntPtr func, IntPtr parms)
        {
            var processEvent = true;
            if (thisPtr != IntPtr.Zero && func != IntPtr.Zero)
            {
                var thisObj = (UObject) thisPtr;
                var funcObj = (UObject) func;

                foreach (var plugin in PluginManager.LoadedPlugins)
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

        public void Initialize()
        {
            Memory = new InternalMemory();

            PluginManager.LoadAll();

            var objectsOffset = MemoryUtility.FindPattern(
                "\x48\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x8B\xD6",
                "xxx????x????x????x????xxx"
            );
            Objects = new FixedObjectStore(MemoryUtility.GetAddressFromOffset(objectsOffset, 7, 3));

            MinHook.CreateHook(Marshal.GetFunctionPointerForDelegate(UObject.ProcessEventInternal), ProcessEventInternalHook, out UObject.ProcessEventInternal);
            MinHook.EnableAllHooks();
        }
    }
}
