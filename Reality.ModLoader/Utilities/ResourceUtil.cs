using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader.Utilities
{
    internal static class ResourceUtil
    {
        public static void Extract(string name, string path)
        {
            name = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.Contains(name)).FirstOrDefault();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
            }
        }

        public static IntPtr LoadLibrary(string name)
        {
            var path = Path.Combine(Bootstrap.ResourcesPath, name);
            Extract(name, path);
            return Win32.LoadLibrary(path);
        }
    }
}
