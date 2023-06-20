namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// 登录输出
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// 登录 token
    /// </summary>
    public LoginToken Token { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    [JsonProperty("userinfo")]
    public LoginUserInfo UserInfo { get; set; }
}

/// <summary>
/// 登录 Token
/// </summary>
public class LoginToken
{
    /// <summary>
    /// token 值
    /// </summary>
    public string TokenValue { get; set; }

    /// <summary>
    /// token key
    /// </summary>
    public string TokenName { get; set; } = "Authorization";
}

/// <summary>
/// 用户信息
/// </summary>
public class LoginUserInfo
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 用户 id
    /// </summary>
    public string Id { get; set; }
}