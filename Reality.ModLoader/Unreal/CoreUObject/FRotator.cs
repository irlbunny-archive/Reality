namespace Reality.ModLoader.Unreal.CoreUObject
{
    /// <summary>
    /// Contains rotation data, offsets should usually never change here.
    /// </summary>
    public class FRotator : UMemoryObject
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
