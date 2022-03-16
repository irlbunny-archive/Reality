namespace Reality.ModLoader.Unreal.CoreUObject
{
    public class UStruct : UField
    {
        public UStruct SuperField => ReadStruct<UStruct>(GetData<int>("USTRUCT_SUPERFIELD_VALUE"));
    }
}
