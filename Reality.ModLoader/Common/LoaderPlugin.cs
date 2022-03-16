using System;
using System.Collections.Generic;

namespace Reality.ModLoader.Common
{
    public abstract class LoaderPlugin : BasePlugin
    {
        public abstract Dictionary<string, Func<object>> Grabbers { get; }
        
        public T Grab<T>(string key)
            => (T) Grabbers[key]();
    }
}
