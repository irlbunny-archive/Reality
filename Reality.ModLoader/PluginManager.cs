using Reality.ModLoader.Common;
using Reality.ModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader
{
    public static class PluginManager
    {
        public static LoaderPlugin LoaderPlugin;
        public static List<GamePlugin> GamePlugins = new();

        /// <summary>
        /// Checks if a game plugin is installed by name.
        /// </summary>
        /// <param name="name">The game plugin name.</param>
        /// <returns>If it's installed, true. Otherwise, false.</returns>
        public static bool IsInstalled(string name)
            => GamePlugins.Any(x => x.Name == name);

        internal static void LoadAll()
        {
            var assemblyFiles = Directory.GetFiles(Loader.PluginsPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var assemblyFile in assemblyFiles)
            {
                Logger.Info($"Loading \"{assemblyFile}\"...");

                var assembly = Assembly.LoadFrom(assemblyFile);
                var pluginTypes = assembly.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && (x.IsSubclassOf(typeof(LoaderPlugin)) || x.IsSubclassOf(typeof(GamePlugin))));
                foreach (var pluginType in pluginTypes)
                {
                    var plugin = (BasePlugin) Activator.CreateInstance(pluginType);
                    if (LoaderPlugin == null && plugin is LoaderPlugin loaderPlugin)
                        LoaderPlugin = loaderPlugin;
                    else if (plugin is GamePlugin gamePlugin)
                        GamePlugins.Add(gamePlugin);

                    Logger.Info($"Loaded plugin: \"{plugin.Name}\" (v{plugin.Version}) by \"{plugin.Author}\"");
                }
            }
        }
    }
}
