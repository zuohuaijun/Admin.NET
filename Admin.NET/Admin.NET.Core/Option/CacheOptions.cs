// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 缓存配置选项
/// </summary>
public sealed class CacheOptions : IConfigurableOptions<CacheOptions>
{
    /// <summary>
    /// 缓存前缀
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// 缓存类型
    /// </summary>
    public string CacheType { get; set; }

    /// <summary>
    /// Redis缓存
    /// </summary>
    public RedisOption Redis { get; set; }

    public void PostConfigure(CacheOptions options, IConfiguration configuration)
    {
        options.Prefix = string.IsNullOrWhiteSpace(options.Prefix) ? "" : options.Prefix.Trim();
    }
}

/// <summary>
/// Redis缓存
/// </summary>
public sealed class RedisOption : RedisOptions
{
    /// <summary>
    /// 最大消息大小
    /// </summary>
    public int MaxMessageSize { get; set; }
}

/// <summary>
/// 集群配置选项
/// </summary>
public sealed class ClusterOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 服务器标识
    /// </summary>
    public string ServerId { get; set; }

    /// <summary>
    /// 服务器IP
    /// </summary>
    public string ServerIp { get; set; }

    /// <summary>
    /// SignalR配置
    /// </summary>
    public ClusterSignalR SignalR { get; set; }

    /// <summary>
    /// 数据保护key
    /// </summary>
    public string DataProtecteKey { get; set; }

    /// <summary>
    /// 是否哨兵模式
    /// </summary>
    public bool IsSentinel { get; set; }

    /// <summary>
    /// 哨兵配置
    /// </summary>
    public StackExchangeSentinelConfig SentinelConfig { get; set; }
}

/// <summary>
/// 集群SignalR配置
/// </summary>
public sealed class ClusterSignalR
{
    /// <summary>
    /// Redis连接字符串
    /// </summary>
    public string RedisConfiguration { get; set; }

    /// <summary>
    /// 缓存前缀
    /// </summary>
    public string ChannelPrefix { get; set; }
}

/// <summary>
/// 哨兵配置
/// </summary>
public sealed class StackExchangeSentinelConfig
{
    /// <summary>
    /// master名称
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// master访问密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 哨兵访问密码
    /// </summary>
    public string SentinelPassword { get; set; }

    /// <summary>
    /// 哨兵端口
    /// </summary>
    public List<string> EndPoints { get; set; }

    /// <summary>
    /// 默认库
    /// </summary>
    public int DefaultDb { get; set; }

    /// <summary>
    /// 主前缀
    /// </summary>
    public string MainPrefix { get; set; }

    /// <summary>
    /// SignalR前缀
    /// </summary>
    public string SignalRChannelPrefix { get; set; }
}