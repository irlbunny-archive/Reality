namespace Reality.ModLoader.Unreal.CoreUObject
{
    public class UStruct : UField
    {
        public UStruct SuperField => ReadStruct<UStruct>(Configuration.GetOffsetFromName("UStruct_SuperField"));
    }
}
