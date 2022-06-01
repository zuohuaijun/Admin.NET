namespace Admin.NET.Core.Service;

/// <summary>
/// 用户登录结果
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 令牌Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public LoginRole RoleInfo { get; set; }
}