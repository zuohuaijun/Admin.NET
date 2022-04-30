using Furion.ConfigurableOptions;

namespace Admin.NET.Core
{
    /// <summary>
    /// 缓存配置选项
    /// </summary>
    public sealed class CacheOptions : IConfigurableOptions
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// Redis连接
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// 键值前缀
        /// </summary>
        public string InstanceName { get; set; }
    }
}