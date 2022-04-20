using Reality.ModLoader.Memory;
using System;
using System.Text;

namespace Reality.ModLoader.Unreal.Core
{
    /// <summary>
    /// Represents a string in UE4, offsets should usually never change here.
    /// </summary>
    public class FString : MemoryStruct
    {
        public IntPtr Data
        {
            get => ReadIntPtr(0);
            set => WriteIntPtr(0, value);
        }

        public int Count
        {
            get => ReadInt32(IntPtr.Size);
            set => WriteInt32(IntPtr.Size, value);
        }

        public int Max
        {
            get => ReadInt32(IntPtr.Size + 4);
            set => WriteInt32(IntPtr.Size + 4, value);
        }

        public string Value
        {
            get
            {
                var buffer = Memory.ReadBytes(Data, 0, Count * 2);
                return Encoding.Unicode.GetString(buffer).TrimEnd('\0');
            }
            set
            {
                if (Data != IntPtr.Zero)
                    FMemory.Free(Data);

                var buffer = Encoding.Unicode.GetBytes(value + '\0');
                Data = FMemory.MallocZero(buffer.Length);
                Memory.WriteBytes(Data, 0, buffer);
                Count = Max = value.Length + 1;
            }
        }

        public FString()
            : base()
        { }

        public FString(IntPtr baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public FString(string value)
            : base()
        {
            Value = value;
        }

        public override int ObjectSize => IntPtr.Size + 8;
    }
}
