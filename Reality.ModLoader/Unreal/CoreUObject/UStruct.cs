namespace Reality.ModLoader.Unreal.CoreUObject
{
    public class UStruct : UField
    {
        public UStruct SuperField => ReadStruct<UStruct>(0x30);
    }
}
