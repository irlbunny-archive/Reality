using Reality.ModLoader.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Reality.ModLoader
{
    public static class Bootstrap
    {
        public static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reality");
        public static string ResourcesPath => Path.Combine(DataPath, "Resources");
        public static string PluginsPath => Path.Combine(DataPath, "Plugins");
        public static string LogsPath => Path.Combine(DataPath, "Logs");

        public static void Main(string[] args)
        {
            // Create all directories.
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(ResourcesPath);
            Directory.CreateDirectory(PluginsPath);
            Directory.CreateDirectory(LogsPath);

            Logger.FilePath = Path.Combine(LogsPath, $"{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt");
            Logger.Info($"Reality (v{Version}), made with <3 by Kaitlyn.");
            Logger.Info("Consider supporting me on Patreon! https://www.patreon.com/join/ItsKaitlyn03");
            Logger.Info("Attempting to initialize ModLoader...");
            Logger.Attempt(Loader.Instance.Initialize, true);
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
