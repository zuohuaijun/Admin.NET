namespace Admin.NET.Core;

/// <summary>
/// 微信相关配置选项
/// </summary>
public sealed class WeChatOptions : IConfigurableOptions
{
    //公众号
    public string WeChatAppId { get; set; }

    public string WeChatAppSecret { get; set; }

    public string EncodingAESKey { get; set; }

    public string Token { get; set; }

    //小程序
    public string WxOpenAppId { get; set; }

    public string WxOpenAppSecret { get; set; }

    public string WxOpenToken { get; set; }

    public string WxOpenEncodingAESKey { get; set; }
}