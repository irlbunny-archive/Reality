using Reality.ModLoader.Memory;
using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ToStringInternalDelegate(IntPtr thisPtr, IntPtr value);
        public static ToStringInternalDelegate ToStringInternal;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        public string Value
        {
            get
            {
                var result = new FString();
                ToStringInternal(BaseAddress, result.BaseAddress);
                return result.Value;
            }
        }

        static FName()
        {
            ToStringInternal = MemoryUtil.GetInternalFuncFromPattern<ToStringInternalDelegate>("\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x00\x83\x79\x04\x00\x48\x8B\xDA\x48\x8B\xF9\x75\x00", "xxxxxxxxx?xxxxxxxxxxx?");
        }

        public override int ObjectSize => 8;
    }
}
