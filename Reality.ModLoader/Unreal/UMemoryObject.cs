using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.Core;
using System;

namespace Reality.ModLoader.Unreal
{
    public abstract class UMemoryObject : MemoryObject, IDisposable
    {
        public UMemoryObject()
        {
            _baseAddress = FMemory.Malloc(ObjectSize, 0);
        }

        public virtual void Dispose()
        {
            if (_baseAddress != IntPtr.Zero)
                FMemory.Free(_baseAddress);
        }

        public override void OnBaseAddressChanged(IntPtr baseAddress)
        {
            if (_baseAddress != IntPtr.Zero)
                FMemory.Free(_baseAddress);
        }
    }
}
