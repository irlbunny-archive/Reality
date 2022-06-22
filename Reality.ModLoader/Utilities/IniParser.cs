using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reality.ModLoader.Utilities
{
    public class IniParser
    {
        private readonly Dictionary<string, string> _values;

        public IniParser(string path)
        {
            if (File.Exists(path))
            {
                _values = File.ReadLines(path)
                    .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    .Select(line => line.Split(new[] { '=' }, 2))
                    .ToDictionary(parts => parts[0].Trim(), parts => parts.Length > 1 ? parts[1].Trim() : null);
            }
            else
                _values = new();
        }

        public bool Exists(string key)
            => _values.ContainsKey(key);

        public string Value(string key, string defaultValue = null)
            => _values.TryGetValue(key, out var value) ? value : defaultValue;

        public string this[string key]
            => Value(key);
    }
}
