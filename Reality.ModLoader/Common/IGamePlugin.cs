using Reality.ModLoader.Unreal.CoreUObject;
using System;

namespace Reality.ModLoader.Common
{
    public interface IGamePlugin : IBasePlugin
    {
        void OnLoad();

        bool OnProcessEvent(UObject obj, UObject func, IntPtr parms);
    }
}
