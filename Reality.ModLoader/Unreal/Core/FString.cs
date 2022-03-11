using Reality.ModLoader.Memory;
using System;
using System.Text;

namespace Reality.ModLoader.Unreal.Core
{
    public class FString : MemoryObject, IDisposable
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
                Data = FMemory.Malloc(buffer.Length, 0);
                Memory.WriteBytes(Data, 0, buffer);
                Count = Max = value.Length + 1;
            }
        }

        public FString()
        {
            _baseAddress = FMemory.Malloc(ObjectSize, 0);
        }

        public FString(IntPtr baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public FString(string value)
            : this()
        {
            Value = value;
        }

        public void Dispose()
        {
            //if (Data != IntPtr.Zero)
            //    FMemory.Free(Data);
            //if (_baseAddress != IntPtr.Zero)
            //    FMemory.Free(_baseAddress);
        }

        public override void OnBaseAddressChanged(IntPtr baseAddress)
        {
            //if (_baseAddress != IntPtr.Zero)
            //    FMemory.Free(_baseAddress);
        }

        public override int ObjectSize => IntPtr.Size + 8;
    }
}
