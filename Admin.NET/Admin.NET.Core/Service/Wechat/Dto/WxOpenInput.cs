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
/// 获取微信用户OpenId
/// </summary>
public class JsCode2SessionInput
{
    /// <summary>
    /// JsCode
    /// </summary>
    [Required(ErrorMessage = "JsCode不能为空"), MinLength(10, ErrorMessage = "JsCode错误")]
    public string JsCode { get; set; }
}

/// <summary>
/// 获取微信用户电话号码
/// </summary>
public class WxPhoneInput : WxOpenIdLoginInput
{
    /// <summary>
    /// Code
    /// </summary>
    [Required(ErrorMessage = "Code不能为空"), MinLength(10, ErrorMessage = "Code错误")]
    public string Code { get; set; }
}

/// <summary>
/// 微信小程序登录
/// </summary>
public class WxOpenIdLoginInput
{
    /// <summary>
    /// OpenId
    /// </summary>
    [Required(ErrorMessage = "微信标识不能为空"), MinLength(10, ErrorMessage = "微信标识错误")]
    public string OpenId { get; set; }
}

/// <summary>
/// 微信手机号登录
/// </summary>
public class WxPhoneLoginInput
{
    /// <summary>
    /// 电话号码
    /// </summary>
    [DataValidation(ValidationTypes.PhoneNumber, ErrorMessage = "电话号码错误")]
    public string PhoneNumber { get; set; }
}

/// <summary>
/// 发送订阅消息
/// </summary>
public class SendSubscribeMessageInput
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
    /// 模板内容，格式形如 { "key1": { "value": any }, "key2": { "value": any } }
    /// </summary>
    [Required(ErrorMessage = "模板内容不能为空")]
    public Dictionary<string, CgibinMessageSubscribeSendRequest.Types.DataItem> Data { get; set; }

    /// <summary>
    /// 跳转小程序类型
    /// </summary>
    public string MiniprogramState { get; set; }

    /// <summary>
    /// 语言类型
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// 点击模板卡片后的跳转页面（仅限本小程序内的页面），支持带参数（示例pages/app/index?foo=bar）
    /// </summary>
    public string MiniProgramPagePath { get; set; }
}

/// <summary>
/// 增加订阅消息模板
/// </summary>
public class AddSubscribeMessageTemplateInput
{
    /// <summary>
    /// 模板标题Id
    /// </summary>
    [Required(ErrorMessage = "模板标题Id不能为空")]
    public string TemplateTitleId { get; set; }

    /// <summary>
    /// 模板关键词列表,例如 [3,5,4]
    /// </summary>
    [Required(ErrorMessage = "模板关键词列表不能为空")]
    public List<int> KeyworkIdList { get; set; }

    /// <summary>
    /// 服务场景描述，15个字以内
    /// </summary>
    [Required(ErrorMessage = "服务场景描述不能为空")]
    public string SceneDescription { get; set; }
}