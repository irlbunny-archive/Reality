using Reality.ModLoader.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        private static Dictionary<int, string> _cachedValues = new();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FNameToStringDelegate(IntPtr thisPtr, IntPtr value);
        public static FNameToStringDelegate ToString_;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        public string Value
        {
            get
            {
                if (!_cachedValues.TryGetValue(ComparisonIndex, out var value))
                {
                    var str = new FString();
                    ToString_(BaseAddress, str.BaseAddress);
                    value = str.Value;
                    str.Dispose();

                    _cachedValues[ComparisonIndex] = value;
                }

                return value;
            }
        }

        static FName()
        {
            ToString_ = MemoryUtil.GetNativeFuncFromPattern<FNameToStringDelegate>("\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x00\x83\x79\x04\x00\x48\x8B\xDA\x48\x8B\xF9\x75\x00", "xxxxxxxxx?xxxxxxxxxxx?");
        }

        public override int ObjectSize => 8;
    }
}
