using System;
using System.Collections.Generic;

namespace Reality.ModLoader.Stores
{
    /// <summary>
    /// Prevents the .NET GC from stealing our (native) delegates.
    /// </summary>
    public static class DelegateStore
    {
        private static HashSet<Delegate> _delegates = new();

        public static void Add(Delegate item)
            => _delegates.Add(item);
    }
}
