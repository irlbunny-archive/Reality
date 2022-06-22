using Reality.ModLoader.GC;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Reality.ModLoader.Utilities.Win32;

namespace Reality.ModLoader.Utilities
{
    public static class MemoryUtil
    {
        public static IntPtr FindPattern(char[] pattern, string mask)
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

        public static IntPtr FindPattern(string pattern, string mask)
            => FindPattern(pattern.ToCharArray(), mask);

        public static IntPtr FindPattern(string pattern)
        {
            var newPattern = new List<char>();
            var newMask = string.Empty;

            var bytes = pattern.Split(' ');
            foreach (var b in bytes)
            {
                if (!b.StartsWith("?"))
                {
                    newPattern.Add((char) Convert.ToByte(b, 16));
                    newMask += "x";
                }
                else
                {
                    newPattern.Add((char) 0);
                    newMask += "?";
                }
            }

            return FindPattern(newPattern.ToArray(), newMask);
        }

        public static T GetInternalFunc<T>(IntPtr target) where T : Delegate
        {
            var func = Marshal.GetDelegateForFunctionPointer<T>(target);
            ObjectPool.Add(func);
            return func;
        }

        public static IntPtr GetAddressFromOffset(IntPtr offset, int p0 = 5, int p1 = 1)
            => new(offset.ToInt64() + p0 + Loader.Memory.ReadInt32(offset, p1));

        public static T GetInternalFuncWithOffset<T>(IntPtr target, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFunc<T>(GetAddressFromOffset(target, p0, p1));

        public static T GetInternalFuncFromPattern<T>(char[] pattern, string mask) where T : Delegate
            => GetInternalFunc<T>(FindPattern(pattern, mask));
        public static T GetInternalFuncFromPattern<T>(string pattern, string mask) where T : Delegate
            => GetInternalFuncFromPattern<T>(pattern, mask);
        public static T GetInternalFuncFromPattern<T>(string pattern) where T : Delegate
            => GetInternalFunc<T>(FindPattern(pattern));

        public static T GetInternalFuncFromPatternWithOffset<T>(char[] pattern, string mask, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFuncWithOffset<T>(FindPattern(pattern, mask), p0, p1);
        public static T GetInternalFuncFromPatternWithOffset<T>(string pattern, string mask, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFuncFromPatternWithOffset<T>(pattern, mask, p0, p1);
        public static T GetInternalFuncFromPatternWithOffset<T>(string pattern, int p0 = 5, int p1 = 1) where T : Delegate
            => GetInternalFuncWithOffset<T>(FindPattern(pattern), p0, p1);
    }
}
