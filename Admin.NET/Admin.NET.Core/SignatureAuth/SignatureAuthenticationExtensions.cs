// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authentication;

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证扩展
/// </summary>
public static class SignatureAuthenticationExtensions
{
    /// <summary>
    /// 注册 Signature 身份验证处理模块
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddSignatureAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddSignatureAuthentication(options => { });
    }

    /// <summary>
    /// 注册 Signature 身份验证处理模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddSignatureAuthentication(this AuthenticationBuilder builder, Action<SignatureAuthenticationOptions> options)
    {
        return builder.AddScheme<SignatureAuthenticationOptions, SignatureAuthenticationHandler>(SignatureAuthenticationDefaults.AuthenticationScheme, options);
    }
}