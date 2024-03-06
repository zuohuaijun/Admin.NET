// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authentication;

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证质询上下文
/// </summary>
public class SignatureChallengeContext : PropertiesContext<SignatureAuthenticationOptions>
{
    public SignatureChallengeContext(HttpContext context,
        AuthenticationScheme scheme,
        SignatureAuthenticationOptions options,
        AuthenticationProperties properties)
        : base(context, scheme, options, properties)
    {
    }

    /// <summary>
    /// 在认证期间出现的异常
    /// </summary>
    public Exception AuthenticateFailure { get; set; }

    /// <summary>
    /// 指定是否已被处理，如果已处理，则跳过默认认证逻辑
    /// </summary>
    public bool Handled { get; private set; }
}