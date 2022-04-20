using Reality.ModLoader.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Reality.ModLoader.Utilities.Win32;

namespace Reality.ModLoader.Unreal.Core
{
    /// <summary>
    /// Handles all memory operations in UE4, also tracks allocations made by Reality/mods internally.
    /// </summary>
    public static class FMemory
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr MallocInternalDelegate(long size, uint alignment);
        /// <summary>
        /// Avoid using this method directly, instead use <see cref="Malloc(long, uint)"/> or <see cref="MallocZero(long, uint)"/>.
        /// </summary>
        public static MallocInternalDelegate MallocInternal;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ReallocInternalDelegate(IntPtr address, long size, uint alignment);
        /// <summary>
        /// Avoid using this method directly, instead use <see cref="Realloc(IntPtr, long, uint)"/>.
        /// </summary>
        public static ReallocInternalDelegate ReallocInternal;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FreeInternalDelegate(IntPtr address);
        /// <summary>
        /// Avoid using this method directly, instead use <see cref="Free(IntPtr)"/>.
        /// </summary>
        public static FreeInternalDelegate FreeInternal;

        /// <summary>
        /// Used for tracking allocations made by Reality/mods.
        /// </summary>
        public static List<IntPtr> InternalAllocations { get; } = new();

        static FMemory()
        {
            MallocInternal = MemoryUtil.GetInternalFuncFromPattern<MallocInternalDelegate>("\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20\x48\x8B\xF9\x8B\xDA\x48\x8B\x0D\x52\x97\x3F\x05\x48\x85\xC9\x75\x0C\xE8\xC0\x77\xFF\xFF\x48\x8B\x0D\x41\x97\x3F\x05\x48\x8B\x01\x44\x8B\xC3\x48\x8B\xD7\x48\x8B\x5C\x24\x30\x48\x83\xC4\x20\x5F\x48\xFF\x60\x10", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            ReallocInternal = MemoryUtil.GetInternalFuncFromPattern<ReallocInternalDelegate>("\x48\x89\x5C\x24\x08\x48\x89\x74\x24\x10\x57\x48\x83\xEC\x20\x48\x8B\xF1\x41\x8B\xD8\x48\x8B\x0D\xFC\x71\x3F\x05\x48\x8B\xFA\x48\x85\xC9\x75\x0C\xE8\x67\x52\xFF\xFF\x48\x8B\x0D\xE8\x71\x3F\x05\x48\x8B\x01\x44\x8B\xCB\x4C\x8B\xC7\x48\x8B\xD6\x48\x8B\x5C\x24\x30\x48\x8B\x74\x24\x38\x48\x83\xC4\x20\x5F\x48\xFF\x60\x18", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            FreeInternal = MemoryUtil.GetInternalFuncFromPattern<FreeInternalDelegate>("\x48\x85\xC9\x74\x2E\x53\x48\x83\xEC\x20\x48\x8B\xD9\x48\x8B\x0D\x54\x26\x40\x05\x48\x85\xC9\x75\x0C\xE8\xC2\x06\x00\x00\x48\x8B\x0D\x43\x26\x40\x05\x48\x8B\x01\x48\x8B\xD3\xFF\x50\x20\x48\x83\xC4\x20\x5B\xC3", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }

        /// <summary>
        /// Dynamically allocates a block of memory.
        /// </summary>
        /// <param name="size">The size of the allocation.</param>
        /// <param name="alignment">The alignment of the allocation.</param>
        /// <returns>A pointer to the newly allocated block of memory.</returns>
        public static IntPtr Malloc(long size, uint alignment = 0)
        {
            var address = MallocInternal(size, alignment);
            InternalAllocations.Add(address);
            return address;
        }

        /// <summary>
        /// Dynamically allocates and zero-fills a block of memory.
        /// </summary>
        /// <param name="size">The size of the allocation.</param>
        /// <param name="alignment">The alignment of the allocation.</param>
        /// <returns>A pointer to the newly allocated and zero-filled block of memory.</returns>
        public static IntPtr MallocZero(long size, uint alignment = 0)
        {
            var address = Malloc(size, alignment);
            RtlFillMemory(address, (uint) size, 0);
            return address;
        }

        /// <summary>
        /// Dynamically reallocates a block of memory.
        /// </summary>
        /// <param name="address">A pointer to an allocated block of memory.</param>
        /// <param name="size">The size of the allocation.</param>
        /// <param name="alignment">The alignment of the allocation.</param>
        /// <returns>A pointer to the newly reallocated block of memory.</returns>
        public static IntPtr Realloc(IntPtr address, long size, uint alignment = 0)
        {
            var newAddress = ReallocInternal(address, size, alignment);
            if (InternalAllocations.Remove(address))
                InternalAllocations.Add(newAddress);
            return newAddress;
        }

        /// <summary>
        /// Dynamically frees a block of memory.
        /// </summary>
        /// <param name="address">A pointer to an allocated block of memory.</param>
        public static void Free(IntPtr address)
        {
            FreeInternal(address);
            InternalAllocations.Remove(address);
        }
    }
}
