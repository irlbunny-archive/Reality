using System;

namespace Reality.ModLoader.Memory
{
    /// <summary>
    /// An interface which lets you easily tell where Reality should read/write memory to.
    /// Yes, this means that if you're crazy enough you could theoretically
    /// make Reality a seperate executable and run it from there.
    /// And, no. I will not do it, do not bother asking me.
    /// </summary>
    public interface IMemory
    {
        byte[] ReadBytes(IntPtr address, int offset, int length);

        sbyte ReadInt8(IntPtr address, int offset);
        byte ReadUInt8(IntPtr address, int offset);

        short ReadInt16(IntPtr address, int offset);
        ushort ReadUInt16(IntPtr address, int offset);

        int ReadInt32(IntPtr address, int offset);
        uint ReadUInt32(IntPtr address, int offset);

        float ReadSingle(IntPtr address, int offset);

        long ReadInt64(IntPtr address, int offset);
        ulong ReadUInt64(IntPtr address, int offset);

        double ReadDouble(IntPtr address, int offset);

        IntPtr ReadIntPtr(IntPtr address, int offset);

        T ReadStruct<T>(IntPtr address, int offset, bool isPtr = true) where T : MemoryObject, new();

        void WriteBytes(IntPtr address, int offset, byte[] bytes);

        void WriteInt8(IntPtr address, int offset, sbyte value);
        void WriteUInt8(IntPtr address, int offset, byte value);

        void WriteInt16(IntPtr address, int offset, short value);
        void WriteUInt16(IntPtr address, int offset, ushort value);

        void WriteInt32(IntPtr address, int offset, int value);
        void WriteUInt32(IntPtr address, int offset, uint value);

        void WriteSingle(IntPtr address, int offset, float value);

        void WriteInt64(IntPtr address, int offset, long value);
        void WriteUInt64(IntPtr address, int offset, ulong value);

        void WriteDouble(IntPtr address, int offset, double value);

        void WriteIntPtr(IntPtr address, int offset, IntPtr value);
    }
}
