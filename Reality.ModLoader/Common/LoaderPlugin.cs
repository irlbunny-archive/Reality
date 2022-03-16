using System;
using System.Collections.Generic;

namespace Reality.ModLoader.Common
{
    public abstract class LoaderPlugin : BasePlugin
    {
        public abstract Dictionary<string, Func<object>> Data { get; }
        private Dictionary<string, object> _cachedData = new();

        public T GetData<T>(string key)
        {
            if (!_cachedData.TryGetValue(key, out var data))
            {
                data = Data[key]();
                _cachedData[key] = data;
            }

            return (T) data;
        }
    }
}
