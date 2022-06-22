using Reality.ModLoader.Utilities;
using System;
using System.IO;
using static Reality.ModLoader.Utilities.Win32;

namespace Reality.ModLoader
{
    public static class Configuration
    {
        public static IniParser Parser { get; private set; }

        static Configuration()
        {
            Parser = new IniParser(Path.Combine(Loader.DataPath, "Configuration.ini"));
        }

        public static IntPtr GetAddressFromName(string name)
        {
            var isOffset = Parser.Exists($"{name}_Offset");
            var isPattern = Parser.Exists($"{name}_Pattern");

            if (isOffset && isPattern)
                throw new Exception($"Offset and pattern exist for \"{name}\", please fix this in your configuration.");
            else if (!isOffset && !isPattern)
                throw new Exception($"Offset and pattern does not exist for \"{name}\", please fix this in your configuration.");

            if (isOffset)
            {
                var offset = Convert.ToInt32(Parser.Value($"{name}_Offset"));
                return GetModuleHandle(null) + offset;
            }

            if (isPattern)
            {
                var hasParts = Parser.Exists($"{name}_Pattern_P0") && Parser.Exists($"{name}_Pattern_P1");
                if (hasParts)
                {
                    var offset = MemoryUtil.FindPattern(Parser.Value($"{name}_Pattern"));
                    var p0 = Convert.ToInt32(Parser.Value($"{name}_Pattern_P0"), 16);
                    var p1 = Convert.ToInt32(Parser.Value($"{name}_Pattern_P1"), 16);
                    return MemoryUtil.GetAddressFromOffset(offset, p0, p1);
                }
                else
                    return MemoryUtil.FindPattern(Parser.Value($"{name}_Pattern"));
            }

            return IntPtr.Zero;
        }

        public static int GetOffsetFromName(string name)
        {
            var hasOffset = Parser.Exists($"{name}_Offset");
            if (!hasOffset)
                throw new Exception($"Offset does not exist for \"{name}\", please fix this in your configuration.");

            return Convert.ToInt32(Parser.Value($"{name}_Offset"), 16);
        }
    }
}
