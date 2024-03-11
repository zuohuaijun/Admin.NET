// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 账号类型枚举
/// </summary>
[Description("账号类型枚举")]
public enum AccountTypeEnum
{
    /// <summary>
    /// 超级管理员
    /// </summary>
    [Description("超级管理员")]
    SuperAdmin = 999,

    /// <summary>
    /// 系统管理员
    /// </summary>
    [Description("系统管理员")]
    SysAdmin = 888,

    /// <summary>
    /// 普通账号
    /// </summary>
    [Description("普通账号")]
    NormalUser = 777,

    /// <summary>
    /// 会员
    /// </summary>
    [Description("会员")]
    Member = 666,
}