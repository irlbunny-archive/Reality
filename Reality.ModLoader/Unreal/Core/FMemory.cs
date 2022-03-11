using Reality.ModLoader.Memory;
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
            Malloc = MemoryUtil.GetNativeFuncFromPattern<FMemoryMallocDelegate>("\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20\x48\x8B\xF9\x8B\xDA\x48\x8B\x0D\x52\x97\x3F\x05\x48\x85\xC9\x75\x0C\xE8\xC0\x77\xFF\xFF\x48\x8B\x0D\x41\x97\x3F\x05\x48\x8B\x01\x44\x8B\xC3\x48\x8B\xD7\x48\x8B\x5C\x24\x30\x48\x83\xC4\x20\x5F\x48\xFF\x60\x10", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            Realloc = MemoryUtil.GetNativeFuncFromPattern<FMemoryReallocDelegate>("\x48\x89\x5C\x24\x08\x48\x89\x74\x24\x10\x57\x48\x83\xEC\x20\x48\x8B\xF1\x41\x8B\xD8\x48\x8B\x0D\xFC\x71\x3F\x05\x48\x8B\xFA\x48\x85\xC9\x75\x0C\xE8\x67\x52\xFF\xFF\x48\x8B\x0D\xE8\x71\x3F\x05\x48\x8B\x01\x44\x8B\xCB\x4C\x8B\xC7\x48\x8B\xD6\x48\x8B\x5C\x24\x30\x48\x8B\x74\x24\x38\x48\x83\xC4\x20\x5F\x48\xFF\x60\x18", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            Free = MemoryUtil.GetNativeFuncFromPattern<FMemoryFreeDelegate>("\x48\x85\xC9\x74\x2E\x53\x48\x83\xEC\x20\x48\x8B\xD9\x48\x8B\x0D\x54\x26\x40\x05\x48\x85\xC9\x75\x0C\xE8\xC2\x06\x00\x00\x48\x8B\x0D\x43\x26\x40\x05\x48\x8B\x01\x48\x8B\xD3\xFF\x50\x20\x48\x83\xC4\x20\x5B\xC3", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }
    }
}
