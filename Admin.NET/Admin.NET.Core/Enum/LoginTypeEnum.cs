namespace Admin.NET.Core;

/// <summary>
/// 登录类型枚举
/// </summary>
public enum LoginTypeEnum
{
    /// <summary>
    /// PC登录
    /// </summary>
    [Description("PC登录")]
    Login = 1,

    /// <summary>
    /// PC退出
    /// </summary>
    [Description("PC退出")]
    Logout = 2,

    /// <summary>
    /// PC注册
    /// </summary>
    [Description("PC注册")]
    Register = 3
}