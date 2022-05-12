using Reality.ModLoader.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reality.ModLoader.Plugins
{
    public static class PluginManager
    {
        public static List<GamePlugin> LoadedPlugins { get; } = new();

        /// <summary>
        /// Checks if a game plugin is loaded by name.
        /// </summary>
        /// <param name="name">The game plugin name.</param>
        /// <returns>If it's loaded, true. Otherwise, false.</returns>
        public static bool IsLoaded(string name)
            => LoadedPlugins.Any(x => x.Name == name);

        internal static void LoadAll()
        {
            var assemblyFiles = Directory.GetFiles(Bootstrap.PluginsPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var assemblyFile in assemblyFiles)
            {
                Logger.Info($"Loading \"{assemblyFile}\"...");

                var assembly = Assembly.LoadFrom(assemblyFile);
                var pluginTypes = assembly.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(GamePlugin)));
                foreach (var pluginType in pluginTypes)
                {
                    var plugin = (BasePlugin) Activator.CreateInstance(pluginType);
                    if (plugin is GamePlugin gamePlugin)
                        LoadedPlugins.Add(gamePlugin);

                    Logger.Info($"Loaded plugin: \"{plugin.Name}\" (v{plugin.Version}) by \"{plugin.Author}\"");
                }
            }
        }
    }
}
