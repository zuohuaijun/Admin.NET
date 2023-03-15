namespace Admin.NET.Core;

/// <summary>
/// 微信通知配置选项
/// </summary>
public sealed class WechatMessageOptions : IConfigurableOptions
{
    /// <summary>
    /// 订阅消息模板Id
    /// </summary>
    public string TemplateId { get; set; }
}