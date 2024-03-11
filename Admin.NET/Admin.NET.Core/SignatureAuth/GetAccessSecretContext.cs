// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authentication;

namespace Admin.NET.Core;

/// <summary>
/// 获取 AccessKey 关联 AccessSecret 方法的上下文
/// </summary>
public class GetAccessSecretContext : BaseContext<SignatureAuthenticationOptions>
{
    public GetAccessSecretContext(HttpContext context,
        AuthenticationScheme scheme,
        SignatureAuthenticationOptions options)
        : base(context, scheme, options)
    {
    }

    /// <summary>
    /// 身份标识
    /// </summary>
    public string AccessKey { get; set; }
}