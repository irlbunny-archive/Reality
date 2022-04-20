using System.Collections.Generic;

namespace Reality.ModLoader.GC
{
    /// <summary>
    /// Stores a bunch of objects, useful for avoiding garbage collection (especially delegates).
    /// </summary>
    public static class ObjectPool
    {
        private static HashSet<object> _objects = new();

        public static void Add(object obj)
            => _objects.Add(obj);

        public static void Remove(object obj)
            => _objects.Remove(obj);
    }
}
