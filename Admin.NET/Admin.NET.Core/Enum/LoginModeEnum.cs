// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 登录模式枚举
/// </summary>
[Description("登录模式枚举")]
public enum LoginModeEnum
{
    /// <summary>
    /// PC模式
    /// </summary>
    [Description("PC模式")]
    PC = 1,

    /// <summary>
    /// APP
    /// </summary>
    [Description("APP")]
    APP = 2
}