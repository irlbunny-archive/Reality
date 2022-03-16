using Reality.ModLoader.Memory;
using System;

namespace Reality.ModLoader.Unreal.Core
{
    /// <summary>
    /// Represents an array in Unreal Engine, offsets should usually never change here.
    /// </summary>
    /// <typeparam name="T">A type which inherits MemoryObject.</typeparam>
    public class TArray<T> : MemoryObject where T : MemoryObject, new()
    {
        private bool _isPtr = true;
        private int _elementSize = IntPtr.Size;

        public IntPtr Data
        {
            get => ReadIntPtr(0);
            set => WriteIntPtr(0, value);
        }

        public int Count
        {
            get => ReadInt32(IntPtr.Size);
            set => WriteInt32(IntPtr.Size, value);
        }

        public int Max
        {
            get => ReadInt32(IntPtr.Size + 4);
            set => WriteInt32(IntPtr.Size + 4, value);
        }

        public TArray()
        { }

        public TArray(IntPtr baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public TArray(IntPtr baseAddress, int elementSize, bool isPtr = true)
        {
            _isPtr = isPtr;
            _elementSize = elementSize;
            _baseAddress = baseAddress;
        }

        public TArray<T> IsPtr(bool isPtr)
        {
            _isPtr = isPtr;
            return this;
        }

        public T GetElement(int index)
            => new() { BaseAddress = _isPtr ? Memory.ReadIntPtr(Data, index * _elementSize) : (Data + (index * _elementSize)) };

        public void SetElement(int index, T value)
            => value.WriteSelf(_isPtr ? Memory.ReadIntPtr(Data, index * _elementSize) : (Data + (index * _elementSize)), _isPtr);

        public T this[int index]
        {
            get => GetElement(index);
            set => SetElement(index, value);
        }

        public void Add(T item)
        {
            Data = FMemory.Realloc(Data, _elementSize * (Count + 1), 0);
            SetElement(Count++, item);
            Max = Count;
        }

        public override int ObjectSize => _elementSize + 8;
    }
}
