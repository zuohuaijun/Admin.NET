// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 支付回调配置选项
/// </summary>
public sealed class PayCallBackOptions : IConfigurableOptions
{
    /// <summary>
    /// 微信支付回调
    /// </summary>
    public string WechatPayUrl { get; set; }

    /// <summary>
    /// 微信退款回调
    /// </summary>
    public string WechatRefundUrl { get; set; }

    /// <summary>
    /// 支付宝支付回调
    /// </summary>
    public string AlipayUrl { get; set; }

    /// <summary>
    /// 支付宝退款回调
    /// </summary>
    public string AlipayRefundUrl { get; set; }
}