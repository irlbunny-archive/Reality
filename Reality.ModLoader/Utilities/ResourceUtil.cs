using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader.Utilities
{
    public static class ResourceUtil
    {
        public static void Extract(Assembly assembly, string name, string path)
        {
            name = assembly.GetManifestResourceNames().Where(x => x.Contains(name)).FirstOrDefault();

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
            }
        }

        public static IntPtr LoadLibrary(Assembly assembly, string name)
        {
            var path = Path.Combine(Loader.ResourcesPath, name);
            Extract(assembly, name, path);
            return Win32.LoadLibrary(path);
        }
    }
}
