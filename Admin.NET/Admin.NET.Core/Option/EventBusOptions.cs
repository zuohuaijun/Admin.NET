// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 事件总线配置选项
/// </summary>
public sealed class EventBusOptions : IConfigurableOptions
{
    /// <summary>
    /// RabbitMQ
    /// </summary>
    public RabbitMQSettings RabbitMQ { get; set; }
}

/// <summary>
/// RabbitMQ
/// </summary>
public sealed class RabbitMQSettings
{
    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 主机
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }
}