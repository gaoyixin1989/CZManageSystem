namespace Plugin.Core
{
    /// <summary>
    /// 插件接口。
    /// </summary>
    public interface IPlugins
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 初始化。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 卸载。
        /// </summary>
        void Unload();
    }
}