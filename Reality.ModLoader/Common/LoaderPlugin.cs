using System;
using System.Collections.Generic;

namespace Reality.ModLoader.Common
{
    public abstract class LoaderPlugin : BasePlugin
    {
        public abstract Dictionary<string, Func<object>> Data { get; }
        
        // TODO(Kaitlyn): Implement caching.
        public T GetData<T>(string key)
            => (T) Data[key]();
    }
}
