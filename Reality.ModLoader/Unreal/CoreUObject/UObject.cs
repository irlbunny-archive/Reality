using Reality.ModLoader.Memory;
using Reality.ModLoader.Stores;
using Reality.ModLoader.Unreal.Core;
using Reality.ModLoader.Utilities;
using System;
using System.Runtime.InteropServices;

namespace Reality.ModLoader.Unreal.CoreUObject
{
    /// <summary>
    /// Represents an object in UE4, offsets should usually not change here.
    /// </summary>
    public class UObject : MemoryObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ProcessEventInternalDelegate(IntPtr thisPtr, IntPtr func, IntPtr parms);
        internal static ProcessEventInternalDelegate ProcessEventInternal;

        public ObjectStore Objects => Loader.Objects;

        public IntPtr VTable => ReadIntPtr(0);
        public int ObjectFlags => ReadInt32(8);
        public int InternalIndex => ReadInt32(0xC);
        public UClass Class => ReadStruct<UClass>(0x10);
        public FName Name => ReadStruct<FName>(0x18, false);
        public UObject Outer => ReadStruct<UObject>(0x20);

        static UObject()
        {
            ProcessEventInternal = MemoryUtil.GetInternalFunc<ProcessEventInternalDelegate>(Configuration.GetAddressFromName("UObject_ProcessEvent"));
        }

        public static implicit operator UObject(IntPtr baseAddress)
            => new UObject { BaseAddress = baseAddress };

        public T Cast<T>() where T : UObject, new()
            => new T { BaseAddress = this.BaseAddress };

        /// <summary>
        /// Checks if this class inherits <paramref name="name"/> and returns true if it is. Otherwise, returns false.
        /// </summary>
        /// <param name="name">The name of the class which this object inherits.</param>
        /// <param name="isSelfClass">Is this object itself a class? (Default: false)</param>
        /// <returns>True if it is <paramref name="name"/>. Otherwise, returns false.</returns>
        public bool IsA(string name, bool isSelfClass = false)
        {
            if (Class == null)
                return false;

            var clazz = isSelfClass ? Cast<UClass>() : Class;
            do
            {
                if (clazz == null)
                    return false;
                if (clazz.GetName() == name)
                    return true;
                if (clazz.SuperField == null)
                    break;

                clazz = clazz.SuperField.Cast<UClass>();
            } while (clazz != null);

            return false;
        }

        /// <summary>
        /// Checks if this class inherits <typeparamref name="T"/> and returns true if it is. Otherwise, returns false.
        /// </summary>
        /// <typeparam name="T">The class to use when checking if this object is that class.</typeparam>
        /// <param name="isSelfClass">Is this object itself a class? (Default: false)</param>
        /// <returns>True if it is <typeparamref name="T"/>. Otherwise, returns false.</returns>
        public bool IsA<T>(bool isSelfClass = false)
            => IsA(typeof(T).Name.TrimStart(new[] { 'F', 'A', 'U' }), isSelfClass);

        /// <summary>
        /// Lets you get the C++ prefix for this object.
        /// </summary>
        /// <returns>The C++ prefix for this object.</returns>
        public string GetPrefix()
        {
            if (IsA("ScriptStruct"))
                return "F";
            if (IsA("Actor", true))
                return "A";

            return "U";
        }

        /// <summary>
        /// Lets you get the name of an object.
        /// </summary>
        /// <param name="withPrefix">Should we include the prefix (e.g. Actor -> AActor) when returning the object name? (Default: false)</param>
        /// <returns>The name of the object.</returns>
        public string GetName(bool withPrefix = false)
            => (withPrefix ? GetPrefix() : string.Empty) + Name.Value;

        /// <summary>
        /// Lets you get the full name of an object.
        /// </summary>
        /// <param name="withClass">Should we include the class name when returning the object name? (Default: true)</param>
        /// <returns>The full name of the object.</returns>
        public string GetFullName(bool withClass = true)
        {
            var name = string.Empty;

            if (Class != null)
            {
                var temp = string.Empty;
                for (var outer = Outer; outer != null; outer = outer.Outer)
                    temp = $"{outer.GetName()}.{temp}";

                if (withClass)
                {
                    name = Class.GetName();
                    name += " ";
                }

                name += temp;
                name += GetName();
            }

            return name;
        }

        /// <summary>
        /// Lets you find any global object and caches the object.
        /// *DO NOT* use this for finding properties, please use <see cref="FindProperty{T}(string, bool, bool)"/> instead.
        /// </summary>
        /// <typeparam name="T">A type which inherits UObject.</typeparam>
        /// <param name="fullName">The full name of an object.</param>
        /// <param name="withClass">Should we include the class name when finding an object? (Default: true)</param>
        /// <returns>If found, a UObject. Otherwise, "null" will be returned.</returns>
        public T FindObject<T>(string fullName, bool withClass = true) where T : UObject, new()
            => Objects.FindObject<T>(fullName, withClass);

        /// <summary>
        /// Lets you find any global object and caches the object.
        /// *DO NOT* use this for finding properties, please use <see cref="FindProperty(string, bool)"/> instead.
        /// </summary>
        /// <param name="fullName">The full name of an object.</param>
        /// <param name="withClass">Should we include the class name when finding an object? (Default: true)</param>
        /// <returns>If found, a UObject. Otherwise, "null" will be returned.</returns>
        public UObject FindObject(string fullName, bool withClass = true)
            => Objects.FindObject(fullName, withClass);

        /// <summary>
        /// A specialized method to find a property and caches the offset.
        /// </summary>
        /// <param name="fullName">The full name of a property.</param>
        /// <param name="withClass">Should we include the class name when finding a property? (Default: true)</param>
        /// <returns>If found, the offset to the property. Otherwise, "0" will be returned.</returns>
        public int FindProperty(string fullName, bool withClass = true)
            => Objects.FindProperty(fullName, withClass);

        /// <summary>
        /// A specialized method to find a property and caches the offset.
        /// </summary>
        /// <typeparam name="T">A type which inherits MemoryObject.</typeparam>
        /// <param name="fullName">The full name of a property.</param>
        /// <param name="withClass">Should we include the class name when finding a property? (Default: true)</param>
        /// <param name="isPtr">Is the result a pointer to a MemoryObject or is it an inline MemoryObject? (Default: true)</param>
        /// <returns>If found, a MemoryObject. Otherwise, "null" will be returned.</returns>
        public T FindProperty<T>(string fullName, bool withClass = true, bool isPtr = true) where T : MemoryObject, new()
            => Objects.FindProperty<T>(BaseAddress, fullName, withClass, isPtr);

        public void ProcessEvent(UObject func, IntPtr parms)
            => ProcessEventInternal(BaseAddress, func.BaseAddress, parms);

        public override int ObjectSize => 0x28;
    }
}
