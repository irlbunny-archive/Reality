namespace Reality.ModLoader.Common
{
    /// <summary>
    /// Do *not* inherit this interface, it is used for internal purposes.
    /// Your plugin will *not* load if you inherit this interface.
    /// Instead, use either <seealso cref="GamePlugin"/> or <seealso cref="LoaderPlugin"/> respectively.
    /// </summary>
    public abstract class BasePlugin
    {
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract string Version { get; }
    }
}
