using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Memory
{
    /// <summary>
    /// Represents the internal memory of the current process.
    /// </summary>
    internal class InternalMemory : IMemory
    {
        public Process Process { get; set; }
        public IntPtr BaseAddress { get; set; }

        public InternalMemory()
        {
            Process = Process.GetCurrentProcess();
            BaseAddress = Process.MainModule.BaseAddress;
        }

        public byte[] ReadBuffer(IntPtr address, int length)
        {
            var buffer = new byte[length];
            Marshal.Copy(address, buffer, 0, buffer.Length);
            return buffer;
        }

        public byte[] ReadBytes(IntPtr address, int offset, int length)
            => ReadBuffer(address + offset, length);

        public sbyte ReadInt8(IntPtr address, int offset)
            => (sbyte) ReadBuffer(address + offset, 1)[0];
        public byte ReadUInt8(IntPtr address, int offset)
            => ReadBuffer(address + offset, 1)[0];

        public short ReadInt16(IntPtr address, int offset)
            => BitConverter.ToInt16(ReadBuffer(address + offset, 2), 0);
        public ushort ReadUInt16(IntPtr address, int offset)
            => BitConverter.ToUInt16(ReadBuffer(address + offset, 2), 0);

        public int ReadInt32(IntPtr address, int offset)
            => BitConverter.ToInt32(ReadBuffer(address + offset, 4), 0);
        public uint ReadUInt32(IntPtr address, int offset)
            => BitConverter.ToUInt32(ReadBuffer(address + offset, 4), 0);


        public float ReadSingle(IntPtr address, int offset)
            => BitConverter.ToSingle(ReadBuffer(address + offset, 4), 0);

        public long ReadInt64(IntPtr address, int offset)
            => BitConverter.ToInt64(ReadBuffer(address + offset, 8), 0);
        public ulong ReadUInt64(IntPtr address, int offset)
            => BitConverter.ToUInt64(ReadBuffer(address + offset, 8), 0);

        public double ReadDouble(IntPtr address, int offset)
            => BitConverter.ToDouble(ReadBuffer(address + offset, 8), 0);

        public IntPtr ReadIntPtr(IntPtr address, int offset)
            => new(BitConverter.ToInt64(ReadBuffer(address + offset, IntPtr.Size), 0));

        public T ReadStruct<T>(IntPtr address, int offset, bool isPtr = true) where T : MemoryObject, new()
        {
            var ptr = isPtr ? ReadIntPtr(address, offset) : address + offset;
            return ptr != IntPtr.Zero ? new() { BaseAddress = ptr } : null;
        }

        public void WriteBuffer(IntPtr address, params byte[] buffer)
            => Marshal.Copy(buffer, 0, address, buffer.Length);

        public void WriteBytes(IntPtr address, int offset, byte[] bytes)
            => WriteBuffer(address + offset, bytes);

        public void WriteInt8(IntPtr address, int offset, sbyte value)
            => WriteBuffer(address + offset, (byte) value);
        public void WriteUInt8(IntPtr address, int offset, byte value)
            => WriteBuffer(address + offset, value);

        public void WriteInt16(IntPtr address, int offset, short value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));
        public void WriteUInt16(IntPtr address, int offset, ushort value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));

        public void WriteInt32(IntPtr address, int offset, int value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));
        public void WriteUInt32(IntPtr address, int offset, uint value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));

        public void WriteSingle(IntPtr address, int offset, float value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));

        public void WriteInt64(IntPtr address, int offset, long value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));
        public void WriteUInt64(IntPtr address, int offset, ulong value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));

        public void WriteDouble(IntPtr address, int offset, double value)
            => WriteBuffer(address + offset, BitConverter.GetBytes(value));

        public void WriteIntPtr(IntPtr address, int offset, IntPtr value)
            => WriteBuffer(address + offset,
                IntPtr.Size == 4 ?
                BitConverter.GetBytes(value.ToInt32()) :
                BitConverter.GetBytes(value.ToInt64())
            );
    }
}
