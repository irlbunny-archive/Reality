using System;

namespace Reality.ModLoader.Stores
{
    public class FixedObjectStore : ObjectStore
    {
        public IntPtr Data => ReadIntPtr(0x10);
        public override int Count => ReadInt32(0x1C);

        public FixedObjectStore(IntPtr baseAddress)
            : base(baseAddress)
        { }

        public override IntPtr GetPtr(int index)
            => Memory.ReadIntPtr(Data, 0x18 * index);
    }
}
