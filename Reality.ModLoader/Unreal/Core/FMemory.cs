using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public static class FMemory
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr FMemoryMallocDelegate(long size, uint alignment);
        public static FMemoryMallocDelegate Malloc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr FMemoryReallocDelegate(IntPtr address, long size, uint alignment);
        public static FMemoryReallocDelegate Realloc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FMemoryFreeDelegate(IntPtr address);
        public static FMemoryFreeDelegate Free;

        static FMemory()
        {
            Malloc = PluginManager.LoaderPlugin.GetData<FMemoryMallocDelegate>("FMEMORY_MALLOC_DELEGATE");
            Realloc = PluginManager.LoaderPlugin.GetData<FMemoryReallocDelegate>("FMEMORY_REALLOC_DELEGATE");
            Free = PluginManager.LoaderPlugin.GetData<FMemoryFreeDelegate>("FMEMORY_FREE_DELEGATE");
        }
    }
}
