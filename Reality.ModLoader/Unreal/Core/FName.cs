using Reality.ModLoader.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        /*private struct CachedName
        {
            public int Number;
            public string Value;
        }

        private static Dictionary<int, CachedName> _cachedNames = new();*/

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FNameToStringDelegate(IntPtr thisPtr, IntPtr value);
        public static FNameToStringDelegate ToString_;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        public string Value
        {
            get
            {
                //void Cache(CachedName cachedName)
                //{
                    var str = new FString();
                    ToString_(BaseAddress, str.BaseAddress);
                    //cachedName.Number = Number;
                    //cachedName.Value = str.Value;
                    var value = str.Value;
                    str.Dispose();

                    //_cachedNames[ComparisonIndex] = cachedName;
                //}

                //if (!_cachedNames.TryGetValue(ComparisonIndex, out var cachedName))
                //    Cache(cachedName);

                //if (cachedName.Number != Number)
                //    Cache(cachedName);

                return /*cachedName.Value*/value;
            }
        }

        static FName()
        {
            ToString_ = MemoryUtil.GetNativeFuncFromPattern<FNameToStringDelegate>("\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x00\x83\x79\x04\x00\x48\x8B\xDA\x48\x8B\xF9\x75\x00", "xxxxxxxxx?xxxxxxxxxxx?");
        }

        public override int ObjectSize => 8;
    }
}
