// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 登录类型枚举
/// </summary>
[Description("登录类型枚举")]
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