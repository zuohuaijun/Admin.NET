namespace Admin.NET.Core.Service;

public class JsCode2SessionInput
{
    /// <summary>
    /// JsCode
    /// </summary>
    [Required(ErrorMessage = "JsCode不能为空"), MinLength(10, ErrorMessage = "JsCode错误")]
    public string JsCode { get; set; }
}

public class WxPhoneInput : WxOpenIdLoginInput
{
    /// <summary>
    /// Code
    /// </summary>
    [Required(ErrorMessage = "Code不能为空"), MinLength(10, ErrorMessage = "Code错误")]
    public string Code { get; set; }
}

public class WxOpenIdLoginInput
{
    /// <summary>
    /// OpenId
    /// </summary>
    [Required(ErrorMessage = "微信标识不能为空"), MinLength(10, ErrorMessage = "微信标识错误")]
    public string OpenId { get; set; }
}

public class WxPhoneLoginInput
{
    /// <summary>
    /// 电话号码
    /// </summary>
    [DataValidation(ValidationTypes.PhoneNumber, ErrorMessage = "电话号码错误")]
    public string PhoneNumber { get; set; }
}

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
    /// 模板内容，格式形如 { "key1": { "value": any }, "key2": { "value": any } }的object
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