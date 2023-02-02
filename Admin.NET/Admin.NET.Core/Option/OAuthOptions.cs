namespace Admin.NET.Core;

/// <summary>
/// 第三方登录授权配置选项
/// </summary>
public sealed class OAuthOptions : IConfigurableOptions
{
    /// <summary>
    /// 微信配置
    /// </summary>
    public Microsoft.AspNetCore.Authentication.OAuth.OAuthOptions Weixin { get; set; }
}