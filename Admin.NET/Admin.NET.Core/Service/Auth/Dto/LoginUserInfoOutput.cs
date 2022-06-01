namespace Admin.NET.Core.Service;

/// <summary>
/// 用户登录信息
/// </summary>
public class LoginUserInfoOutput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 账号名称
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 角色集合
    /// </summary>
    public List<LoginRole> Roles { get; set; }
}

public class LoginRole
{
    /// <summary>
    /// 角色名称
    /// </summary>
    /// <example>admin</example>
    public string RoleName { get; set; }

    /// <summary>
    /// 角色编码
    /// </summary>
    /// <example>123456</example>
    public string Value { get; set; }
}