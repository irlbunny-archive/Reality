using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader.Resources
{
    internal static class ResourceUtility
    {
        public static void WriteResourceToFile(string name, string path)
        {
            name = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(x => x.Contains(name)).FirstOrDefault();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
            }
        }
    }
}
