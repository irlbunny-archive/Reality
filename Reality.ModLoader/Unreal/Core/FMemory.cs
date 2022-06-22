using Reality.ModLoader.Utilities;
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
        internal delegate IntPtr MallocInternalDelegate(long size, uint alignment);
        internal static MallocInternalDelegate MallocInternal;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr ReallocInternalDelegate(IntPtr address, long size, uint alignment);
        internal static ReallocInternalDelegate ReallocInternal;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void FreeInternalDelegate(IntPtr address);
        internal static FreeInternalDelegate FreeInternal;

        /// <summary>
        /// Used for tracking allocations made by Reality/mods.
        /// </summary>
        public static List<IntPtr> InternalAllocations { get; } = new();

        static FMemory()
        {
            MallocInternal = MemoryUtil.GetInternalFunc<MallocInternalDelegate>(Configuration.GetAddressFromName("FMemory_Malloc"));
            ReallocInternal = MemoryUtil.GetInternalFunc<ReallocInternalDelegate>(Configuration.GetAddressFromName("FMemory_Realloc"));
            FreeInternal = MemoryUtil.GetInternalFunc<FreeInternalDelegate>(Configuration.GetAddressFromName("FMemory_Free"));
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
