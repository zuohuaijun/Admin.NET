// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 用户登录结果
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// 令牌Token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 刷新Token
    /// </summary>
    public string RefreshToken { get; set; }
}