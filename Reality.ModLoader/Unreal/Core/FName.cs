using Reality.ModLoader.Memory;
using Reality.ModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        private static Dictionary<int, string> _cachedValues = new();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ToStringInternalDelegate(IntPtr thisPtr, IntPtr value);
        internal static ToStringInternalDelegate ToStringInternal;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        public string Value
        {
            get
            {
                if (!_cachedValues.TryGetValue(ComparisonIndex, out var value))
                {
                    using (var str = new FString())
                    {
                        ToStringInternal(BaseAddress, str.BaseAddress);

                        value = str.Value;
                        _cachedValues[ComparisonIndex] = value;
                    }
                }

                return value;
            }
        }

        static FName()
        {
            ToStringInternal = MemoryUtil.GetInternalFunc<ToStringInternalDelegate>(Configuration.GetAddressFromName("FName_ToString"));
        }

        public override int ObjectSize => 8;
    }
}
