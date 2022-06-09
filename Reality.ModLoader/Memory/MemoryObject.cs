using System;

namespace Reality.ModLoader.Memory
{
    /// <summary>
    /// It's like <see cref="System.Object"/> but for raw memory.
    /// What more do you need to know? :')
    /// </summary>
    public abstract class MemoryObject
    {
        public IMemory Memory => Loader.Memory;

        protected IntPtr _baseAddress;
        public IntPtr BaseAddress
        {
            get => _baseAddress;
            set
            {
                OnBaseAddressUpdated(value);
                _baseAddress = value;
            }
        }

        public byte[] ReadBytes(int offset, int length)
            => Memory.ReadBytes(BaseAddress, offset, length);

        public sbyte ReadInt8(int offset)
            => Memory.ReadInt8(BaseAddress, offset);
        public byte ReadUInt8(int offset)
            => Memory.ReadUInt8(BaseAddress, offset);

        public short ReadInt16(int offset)
            => Memory.ReadInt16(BaseAddress, offset);
        public ushort ReadUInt16(int offset)
            => Memory.ReadUInt16(BaseAddress, offset);

        public int ReadInt32(int offset)
            => Memory.ReadInt32(BaseAddress, offset);
        public uint ReadUInt32(int offset)
            => Memory.ReadUInt32(BaseAddress, offset);

        public float ReadSingle(int offset)
            => Memory.ReadSingle(BaseAddress, offset);

        public long ReadInt64(int offset)
            => Memory.ReadInt64(BaseAddress, offset);
        public ulong ReadUInt64(int offset)
            => Memory.ReadUInt64(BaseAddress, offset);

        public double ReadDouble(int offset)
            => Memory.ReadDouble(BaseAddress, offset);

        public IntPtr ReadIntPtr(int offset)
            => Memory.ReadIntPtr(BaseAddress, offset);

        public T ReadStruct<T>(int offset, bool isPtr = true) where T : MemoryObject, new()
            => Memory.ReadStruct<T>(BaseAddress, offset, isPtr);

        public void WriteBytes(int offset, byte[] bytes)
            => Memory.WriteBytes(BaseAddress, offset, bytes);

        public void WriteInt8(int offset, sbyte value)
            => Memory.WriteInt8(BaseAddress, offset, value);
        public void WriteUInt8(int offset, byte value)
            => Memory.WriteUInt8(BaseAddress, offset, value);

        public void WriteInt16(int offset, short value)
            => Memory.WriteInt16(BaseAddress, offset, value);
        public void WriteUInt16(int offset, ushort value)
            => Memory.WriteUInt16(BaseAddress, offset, value);

        public void WriteInt32(int offset, int value)
            => Memory.WriteInt32(BaseAddress, offset, value);
        public void WriteUInt32(int offset, uint value)
            => Memory.WriteUInt32(BaseAddress, offset, value);

        public void WriteSingle(int offset, float value)
            => Memory.WriteSingle(BaseAddress, offset, value);

        public void WriteInt64(int offset, long value)
            => Memory.WriteInt64(BaseAddress, offset, value);
        public void WriteUInt64(int offset, ulong value)
            => Memory.WriteUInt64(BaseAddress, offset, value);

        public void WriteDouble(int offset, double value)
            => Memory.WriteDouble(BaseAddress, offset, value);

        public void WriteIntPtr(int offset, IntPtr value)
            => Memory.WriteIntPtr(BaseAddress, offset, value);

        public void WriteStruct(IntPtr address, bool isPtr = true)
        {
            if (isPtr)
                Memory.WriteIntPtr(address, 0, BaseAddress);
            else
                Memory.WriteBytes(address, 0, Memory.ReadBytes(BaseAddress, 0, ObjectSize));
        }

        public virtual void OnBaseAddressUpdated(IntPtr baseAddress)
        { }

        public abstract int ObjectSize { get; }
    }
}
