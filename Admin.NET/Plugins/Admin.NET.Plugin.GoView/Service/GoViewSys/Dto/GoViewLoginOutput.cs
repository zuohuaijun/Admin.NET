namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// 登录输出
/// </summary>
public class GoViewLoginOutput
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public GoViewLoginUserInfo Userinfo { get; set; }

    /// <summary>
    /// Token
    /// </summary>
    public GoViewLoginToken Token { get; set; }
}

/// <summary>
/// 登录 Token
/// </summary>
public class GoViewLoginToken
{
    /// <summary>
    /// Token 名
    /// </summary>
    public string TokenName { get; set; } = "Authorization";

    /// <summary>
    /// Token 值
    /// </summary>
    public string TokenValue { get; set; }
}

/// <summary>
/// 用户信息
/// </summary>
public class GoViewLoginUserInfo
{
    /// <summary>
    /// 用户 Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }
}