using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Memory
{
    /// <summary>
    /// Represents the internal memory of this process.
    /// </summary>
    internal unsafe class InternalMemory : IMemory
    {
        public byte[] ReadBytes(IntPtr address, int offset, int length)
        {
            var bytes = new byte[length];
            Marshal.Copy(address + offset, bytes, 0, bytes.Length);
            return bytes;
        }

        public sbyte ReadInt8(IntPtr address, int offset)
            => (sbyte) ReadUInt8(address, offset);
        public byte ReadUInt8(IntPtr address, int offset)
            => *((byte*) address + offset);

        public short ReadInt16(IntPtr address, int offset)
            => (short) ReadUInt16(address, offset);
        public ushort ReadUInt16(IntPtr address, int offset)
            => (ushort) (ReadUInt8(address, offset) | ReadUInt8(address, offset + 1) << 8);

        public int ReadInt32(IntPtr address, int offset)
            => (int) ReadUInt32(address, offset);
        public uint ReadUInt32(IntPtr address, int offset)
            => (uint) (ReadUInt16(address, offset) | ReadUInt16(address, offset + 2) << 16);


        public float ReadSingle(IntPtr address, int offset)
            => BitConverter.ToSingle(ReadBytes(address, offset, 4), 0);

        public long ReadInt64(IntPtr address, int offset)
            => (long) ReadUInt64(address, offset);
        public ulong ReadUInt64(IntPtr address, int offset)
            => (ulong) ReadUInt32(address, offset) | (ulong) ReadUInt32(address, offset + 4) << 32;

        public double ReadDouble(IntPtr address, int offset)
            => BitConverter.ToDouble(ReadBytes(address, offset, 8), 0);

        public IntPtr ReadIntPtr(IntPtr address, int offset)
            => new(IntPtr.Size == 4 ? ReadInt32(address, offset) : ReadInt64(address, offset));

        public T ReadStruct<T>(IntPtr address, int offset, bool isPtr = true) where T : MemoryObject, new()
        {
            var ptr = isPtr ? ReadIntPtr(address, offset) : address + offset;
            return ptr != IntPtr.Zero ? new() { BaseAddress = ptr } : null;
        }

        public void WriteBytes(IntPtr address, int offset, byte[] bytes)
            => Marshal.Copy(bytes, 0, address + offset, bytes.Length);

        public void WriteInt8(IntPtr address, int offset, sbyte value)
            => WriteUInt8(address, offset, (byte) value);
        public void WriteUInt8(IntPtr address, int offset, byte value)
            => *((byte*) address + offset) = value;

        public void WriteInt16(IntPtr address, int offset, short value)
            => WriteUInt16(address, offset, (ushort) value);
        public void WriteUInt16(IntPtr address, int offset, ushort value)
        {
            WriteUInt8(address, offset, (byte) value);
            WriteUInt8(address, offset + 1, (byte) (value >> 8));
        }

        public void WriteInt32(IntPtr address, int offset, int value)
            => WriteUInt32(address, offset, (uint) value);
        public void WriteUInt32(IntPtr address, int offset, uint value)
        {
            WriteUInt16(address, offset, (ushort) value);
            WriteUInt16(address, offset + 2, (ushort) (value >> 16));
        }

        public void WriteSingle(IntPtr address, int offset, float value)
            => WriteBytes(address, offset, BitConverter.GetBytes(value));

        public void WriteInt64(IntPtr address, int offset, long value)
            => WriteUInt64(address, offset, (ulong) value);
        public void WriteUInt64(IntPtr address, int offset, ulong value)
        {
            WriteUInt32(address, offset, (uint) value);
            WriteUInt32(address, offset + 4, (uint) (value >> 32));
        }

        public void WriteDouble(IntPtr address, int offset, double value)
            => WriteBytes(address, offset, BitConverter.GetBytes(value));

        public void WriteIntPtr(IntPtr address, int offset, IntPtr value)
        {
            if (IntPtr.Size == 4)
                WriteInt32(address, offset, value.ToInt32());
            else
                WriteInt64(address, offset, value.ToInt64());
        }
    }
}
