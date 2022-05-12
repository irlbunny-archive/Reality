using Reality.ModLoader.Memory;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using System;

namespace Reality.ModLoader.Plugins
{
    public abstract class GamePlugin : BasePlugin
    {
        public IMemory Memory => Loader.Memory;
        public ObjectStore Objects => Loader.Objects;

        public virtual bool OnProcessEvent(UObject obj, UObject func, IntPtr parms)
            => true;
    }
}
