using Reality.ModLoader.Memory;
using Reality.ModLoader.Utilities;
using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ToStringInternalDelegate(IntPtr thisPtr, IntPtr value);
        internal static ToStringInternalDelegate ToStringInternal;

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
            ToStringInternal = MemoryUtil.GetInternalFunc<ToStringInternalDelegate>(Configuration.GetAddressFromName("FName_ToString"));
        }

        public override int ObjectSize => 8;
    }
}
