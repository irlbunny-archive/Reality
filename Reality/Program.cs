using Reality.ModLoader.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static Reality.Utilities.Win32;

namespace Reality
{
    public class Program
    {
        public static string Version => $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";

        public static void Main(string[] args)
        {
            Logger.Info($"Reality ModLoader ({Version})");

            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(args[0], string.Join(" ", args.Skip(1)))
            };
            process.Start();

            LoadLibrary(process.Id, Path.Combine(Environment.CurrentDirectory, "Reality.Bootstrap.dll"));
        }
    }
}
