namespace Admin.NET.Core;

/// <summary>
/// 微信相关配置选项
/// </summary>
public sealed class WechatOptions : IConfigurableOptions
{
    //公众号
    public string WechatAppId { get; set; }

    public string WechatAppSecret { get; set; }

    //小程序
    public string WxOpenAppId { get; set; }

    public string WxOpenAppSecret { get; set; }
}