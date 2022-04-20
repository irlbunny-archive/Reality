using Reality.ModLoader.Utilities;
using System;
using System.IO;
using System.Reflection;

namespace Reality.ModLoader
{
    public static class Bootstrap
    {
        public static void Main(string[] args)
        {
            try
            {
                Loader.Instance.Initialize(); // Attempt to initialize/start our app.
            }
            catch (Exception ex)
            {
                Logger.Error($"An exception has occured in Reality, please report this exception to the Reality developers!\n\n{ex}");
                Environment.Exit(-1);
            }
        }

        // Allows us to load assemblies from where the executing assembly is located at.
        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assemblyPath = Path.Combine(folderPath, $"{new AssemblyName(args.Name).Name}.dll");
            return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
        }

        public static int HostedMain(string args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            Main(new[] { args });
            return 0;
        }
    }
}
