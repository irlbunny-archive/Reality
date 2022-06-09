using Reality.ModLoader.Unreal.Core;
using System;

namespace Reality.ModLoader.Memory
{
    /// <summary>
    /// This just handles allocating a block of memory (depending on ObjectSize) upon the constructor being called.
    /// If the BaseAddress is changed externally (and it's not internally allocated by Reality), then it will free our internally allocated block of memory.
    /// </summary>
    public abstract class MemoryStruct : MemoryObject
    {
        public MemoryStruct()
        {
            _baseAddress = FMemory.MallocZero(ObjectSize);
        }

        public override void OnBaseAddressUpdated(IntPtr baseAddress)
        {
            if (_baseAddress != IntPtr.Zero && FMemory.InternalAllocations.Contains(_baseAddress))
                FMemory.Free(_baseAddress);
        }
    }
}
