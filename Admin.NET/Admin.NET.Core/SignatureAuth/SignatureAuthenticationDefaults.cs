// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证处理程序相关的默认值
/// </summary>
public static class SignatureAuthenticationDefaults
{
    /// <summary>
    /// SignatureAuthenticationOptions.AuthenticationScheme 使用的默认值
    /// </summary>
    public const string AuthenticationScheme = "Signature";

    /// <summary>
    /// 附加在 HttpContext Item 中验证失败消息的 Key
    /// </summary>
    public const string AuthenticateFailMsgKey = "SignatureAuthenticateFailMsg";
}