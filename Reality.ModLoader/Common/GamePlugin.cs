using Reality.ModLoader.Memory;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.CoreUObject;
using System;

namespace Reality.ModLoader.Common
{
    public abstract class GamePlugin : BasePlugin
    {
        public IMemory Memory => Loader.Instance.Memory;
        public ObjectStore Objects => Loader.Instance.Objects;

        public abstract bool OnProcessEvent(UObject obj, UObject func, IntPtr parms);
    }
}
