namespace Admin.NET.Core;

/// <summary>
/// 账号类型枚举
/// </summary>
public enum UserTypeEnum
{
    /// <summary>
    /// 管理员
    /// </summary>
    [Description("管理员")]
    Admin = 1,

    /// <summary>
    /// 普通账号
    /// </summary>
    [Description("普通账号")]
    User = 2,

    /// <summary>
    /// 游客
    /// </summary>
    [Description("游客")]
    Nano = 3,

    /// <summary>
    /// 超级管理员
    /// </summary>
    [Description("超级管理员")]
    SuperAdmin = 999,
}