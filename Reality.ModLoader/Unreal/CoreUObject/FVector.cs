namespace Reality.ModLoader.Unreal.CoreUObject
{
    /// <summary>
    /// Contains vector data, offsets should usually never change here.
    /// </summary>
    public class FVector : UMemoryObject
    {
        public float X
        {
            get => ReadSingle(0);
            set => WriteSingle(0, value);
        }

        public float Y
        {
            get => ReadSingle(4);
            set => WriteSingle(4, value);
        }

        public float Z
        {
            get => ReadSingle(8);
            set => WriteSingle(8, value);
        }

        public override int ObjectSize => 0xC;
    }
}
