namespace Reality.ModLoader.Plugins
{
    /// <summary>
    /// Do *not* inherit this class, it is used for internal purposes.
    /// Your plugin will *not* load if you inherit this class.
    /// Instead, use <seealso cref="UnrealPlugin"/>.
    /// </summary>
    public abstract class BasePlugin
    {
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract string Version { get; }
    }
}
