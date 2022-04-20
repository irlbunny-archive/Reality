namespace Reality.ModLoader.Unreal.CoreUObject
{
    public class UProperty : UField
    {
        public int Offset => ReadInt32(0x44);
    }
}
