using System;
using System.Runtime.InteropServices;
using static Reality.ModLoader.Utilities.Win32;

namespace Reality.ModLoader.Unreal.Core
{
    public static class FMemory
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr FMemoryMallocDelegate(long size, uint alignment);
        public static FMemoryMallocDelegate Malloc_;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr FMemoryReallocDelegate(IntPtr address, long size, uint alignment);
        public static FMemoryReallocDelegate Realloc_;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FMemoryFreeDelegate(IntPtr address);
        public static FMemoryFreeDelegate Free_;

        static FMemory()
        {
            Malloc_ = PluginManager.LoaderPlugin.GetData<FMemoryMallocDelegate>("FMEMORY_MALLOC_DELEGATE");
            Realloc_ = PluginManager.LoaderPlugin.GetData<FMemoryReallocDelegate>("FMEMORY_REALLOC_DELEGATE");
            Free_ = PluginManager.LoaderPlugin.GetData<FMemoryFreeDelegate>("FMEMORY_FREE_DELEGATE");
        }

        public static IntPtr Malloc(long size, uint alignment)
        {
            var address = Malloc_(size, alignment);
            RtlFillMemory(address, (uint) size, 0);
            return address;
        }

        public static IntPtr Realloc(IntPtr address, long size, uint alignment)
            => Realloc_(address, size, alignment);

        public static void Free(IntPtr address)
            => Free_(address);
    }
}
