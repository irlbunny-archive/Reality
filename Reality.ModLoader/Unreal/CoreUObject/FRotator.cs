using Reality.ModLoader.Memory;

namespace Reality.ModLoader.Unreal.CoreUObject
{
    /// <summary>
    /// Represents rotation data in UE4, offsets should usually never change here.
    /// </summary>
    public class FRotator : MemoryStruct
    {
        public float Pitch
        {
            get => ReadSingle(0);
            set => WriteSingle(0, value);
        }

        public float Yaw
        {
            get => ReadSingle(4);
            set => WriteSingle(4, value);
        }

        public float Roll
        {
            get => ReadSingle(8);
            set => WriteSingle(8, value);
        }

        public override int ObjectSize => 0xC;
    }
}
