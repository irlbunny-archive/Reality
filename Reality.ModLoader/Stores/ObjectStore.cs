using Reality.ModLoader.Memory;
using Reality.ModLoader.Unreal.CoreUObject;
using System;
using System.Collections.Generic;

namespace Reality.ModLoader.Stores
{
    public abstract class ObjectStore : MemoryObject
    {
        private static Dictionary<string, UObject> _cachedObjects = new();
        private static Dictionary<string, int> _cachedOffsets = new();

        public abstract int Count { get; }

        public ObjectStore(IntPtr baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public abstract IntPtr GetPtr(int index);

        public T GetObject<T>(int index) where T : UObject, new()
        {
            var ptr = GetPtr(index);
            return ptr != IntPtr.Zero ? new() { BaseAddress = ptr } : null;
        }

        public UObject this[int index] => GetObject<UObject>(index);

        public T FindObject<T>(string fullName, bool withClass = true, bool withCache = true) where T : UObject, new()
        {
            UObject FindObjectInternal()
            {
                for (var i = 0; i < Count; i++)
                {
                    var obj = GetObject<T>(i);
                    if (obj != null)
                    {
                        if (obj.GetFullName(withClass) == fullName)
                            return obj;
                    }
                }

                return null;
            }

            UObject obj;
            if (withCache && !_cachedObjects.TryGetValue(fullName, out obj))
            {
                _cachedObjects[fullName] = FindObjectInternal();
                obj = _cachedObjects[fullName];
            }
            else
                obj = FindObjectInternal();

            return obj.Cast<T>();
        }

        public UObject FindObject(string fullName, bool withClass = true, bool withCache = true)
            => FindObject<UObject>(fullName, withClass, withCache);

        public int FindProperty(string fullName, bool withClass = true)
        {
            if (!_cachedOffsets.TryGetValue(fullName, out var offset))
            {
                var property = FindObject<UProperty>(fullName, withClass);
                if (property != null)
                {
                    offset = property.Offset;
                    _cachedOffsets[fullName] = offset;
                }
                else
                    return 0;
            }

            return offset;
        }

        public T FindProperty<T>(IntPtr baseAddress, string fullName, bool withClass = true, bool isPtr = true) where T : MemoryObject, new()
        {
            var offset = FindProperty(fullName, withClass);
            if (offset == 0)
                return null;

            return new() { BaseAddress = isPtr ? Loader.Instance.Memory.ReadIntPtr(baseAddress, offset) : (baseAddress + offset) };
        }

        public override int ObjectSize => 0;
    }
}
