namespace Admin.NET.Core;

/// <summary>
/// 账号类型枚举
/// </summary>
public enum AccountTypeEnum
{
    /// <summary>
    /// 游客
    /// </summary>
    [Description("游客")]
    None = 0,

    /// <summary>
    /// 普通账号
    /// </summary>
    [Description("普通账号")]
    User = 1,

    /// <summary>
    /// 管理员
    /// </summary>
    [Description("管理员")]
    Admin = 2,

    /// <summary>
    /// 超级管理员
    /// </summary>
    [Description("超级管理员")]
    SuperAdmin = 99999,
}