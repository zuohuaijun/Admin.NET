// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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