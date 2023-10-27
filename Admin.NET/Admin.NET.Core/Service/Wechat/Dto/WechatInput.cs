// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 生成网页授权Url
/// </summary>
public class GenAuthUrlInput
{
    /// <summary>
    /// RedirectUrl
    /// </summary>
    public string RedirectUrl { get; set; }

    /// <summary>
    /// Scope
    /// </summary>
    public string Scope { get; set; }
}

/// <summary>
/// 获取微信用户OpenId
/// </summary>
public class WechatOAuth2Input
{
    /// <summary>
    /// Code
    /// </summary>
    [Required(ErrorMessage = "Code不能为空"), MinLength(10, ErrorMessage = "Code错误")]
    public string Code { get; set; }
}

/// <summary>
/// 微信用户登录
/// </summary>
public class WechatUserLogin
{
    /// <summary>
    /// OpenId
    /// </summary>
    [Required(ErrorMessage = "微信标识不能为空"), MinLength(10, ErrorMessage = "微信标识长错误")]
    public string OpenId { get; set; }
}

/// <summary>
/// 获取配置签名
/// </summary>
public class SignatureInput
{
    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; }
}

/// <summary>
/// 获取消息模板列表
/// </summary>
public class MessageTemplateSendInput
{
    /// <summary>
    /// 订阅模板Id
    /// </summary>
    [Required(ErrorMessage = "订阅模板Id不能为空")]
    public string TemplateId { get; set; }

    /// <summary>
    /// 接收者的OpenId
    /// </summary>
    [Required(ErrorMessage = "接收者的OpenId不能为空")]
    public string ToUserOpenId { get; set; }

    /// <summary>
    /// 模板数据，格式形如 { "key1": { "value": any }, "key2": { "value": any } }
    /// </summary>
    [Required(ErrorMessage = "模板数据不能为空")]
    public Dictionary<string, CgibinMessageSubscribeSendRequest.Types.DataItem> Data { get; set; }

    /// <summary>
    /// 模板跳转链接
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar）
    /// </summary>
    public string MiniProgramPagePath { get; set; }
}

/// <summary>
/// 删除消息模板
/// </summary>
public class DeleteMessageTemplateInput
{
    /// <summary>
    /// 订阅模板Id
    /// </summary>
    [Required(ErrorMessage = "订阅模板Id不能为空")]
    public string TemplateId { get; set; }
}