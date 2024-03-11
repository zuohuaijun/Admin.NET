// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authentication;

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证已验证上下文
/// </summary>
public class SignatureValidatedContext : ResultContext<SignatureAuthenticationOptions>
{
    public SignatureValidatedContext(HttpContext context,
        AuthenticationScheme scheme,
        SignatureAuthenticationOptions options)
        : base(context, scheme, options)
    {
    }

    /// <summary>
    /// 身份标识
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string AccessSecret { get; set; }
}