namespace Reality.ModLoader.Common
{
    /// <summary>
    /// Do *not* inherit this interface, it is used for internal purposes.
    /// Your plugin will *not* load if you inherit this interface.
    /// Instead, use either <seealso cref="IGamePlugin"/> or <seealso cref="ILoaderPlugin"/> respectively.
    /// </summary>
    public interface IBasePlugin
    {
        public string Name { get; }
        public string Author { get; }
        public string Version { get; }
    }
}
