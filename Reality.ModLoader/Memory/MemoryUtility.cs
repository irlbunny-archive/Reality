using Reality.ModLoader.GC;
using System;
using System.Runtime.InteropServices;
using static Reality.ModLoader.Utilities.Win32;

namespace Reality.ModLoader.Memory
{
    public static class MemoryUtility
    {
        public static IntPtr FindPattern(string pattern, string mask)
        {
            var process = GetCurrentProcess();
            var handle = GetModuleHandle(null);
            GetModuleInformation(process, handle, out var info, (uint) Marshal.SizeOf<MODULEINFO>());

            var buffer = new byte[info.SizeOfImage];
            if (ReadProcessMemory(process, info.lpBaseOfDll, buffer, (int) info.SizeOfImage, out _))
            {
                for (var i = 0; i < info.SizeOfImage; i++)
                {
                    var found = true;

                    for (var j = 0; j < mask.Length; j++)
                    {
                        found = mask[j] == '?' || buffer[j + i] == pattern[j];
                        if (!found)
                            break;
                    }

                    if (found)
                        return IntPtr.Add(handle, i);
                }
            }

            return IntPtr.Zero;
        }

        public static T GetInternalFunc<T>(IntPtr target) where T : Delegate
        {
            var func = Marshal.GetDelegateForFunctionPointer<T>(target);
            ObjectPool.Add(func);
            return func;
        }

        public static IntPtr GetAddressFromOffset(IntPtr offset, int p0 = 5, int p1 = 1)
            => new(offset.ToInt64() + p0 + Loader.Instance.Memory.ReadInt32(offset, p1));

        public static T GetInternalFuncWithOffset<T>(IntPtr target, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFunc<T>(GetAddressFromOffset(target, p0, p1));

        public static T GetInternalFuncFromPattern<T>(string pattern, string mask) where T : Delegate
            => GetInternalFunc<T>(FindPattern(pattern, mask));
        public static T GetInternalFuncFromPatternWithOffset<T>(string pattern, string mask, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFuncWithOffset<T>(FindPattern(pattern, mask), p0, p1);
    }
}
