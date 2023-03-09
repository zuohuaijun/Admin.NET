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