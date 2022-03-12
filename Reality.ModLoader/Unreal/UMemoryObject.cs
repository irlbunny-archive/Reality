using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.Core;
using System;

namespace Reality.ModLoader.Unreal
{
    /// <summary>
    /// This just handles allocating a block of memory (depending on ObjectSize) upon the constructor being called.
    /// If the BaseAddress is changed externally, then it will free our self-allocated block of memory.
    /// </summary>
    public abstract class UMemoryObject : MemoryObject
    {
        private bool _isSelfAllocated;

        public UMemoryObject()
        {
            _baseAddress = FMemory.Malloc(ObjectSize, 0);
            _isSelfAllocated = true;
        }

        public override void OnBaseAddressChanged(IntPtr baseAddress)
        {
            if (_isSelfAllocated && _baseAddress != IntPtr.Zero)
                FMemory.Free(_baseAddress);

            _isSelfAllocated = false;
        }
    }
}
