using Reality.ModLoader.Memory;
using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.Core
{
    public class FName : MemoryObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FNameToStringDelegate(IntPtr thisPtr, IntPtr value);
        public static FNameToStringDelegate ToString_;

        public int ComparisonIndex => ReadInt32(0);
        public int Number => ReadInt32(4);

        public string Value
        {
            get
            {
                var _string = new FString();
                ToString_(BaseAddress, _string.BaseAddress);
                return _string.Value;
            }
        }

        static FName()
        {
            ToString_ = PluginManager.LoaderPlugin.GetData<FNameToStringDelegate>("FNAME_TOSTRING_DELEGATE");
        }

        public override int ObjectSize => 8;
    }
}
