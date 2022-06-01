namespace Admin.NET.Core;

/// <summary>
/// 角色类型枚举
/// </summary>
public enum RoleTypeEnum
{
    /// <summary>
    /// 集团角色
    /// </summary>
    [Description("集团角色")]
    Group,

    /// <summary>
    /// 加盟商角色
    /// </summary>
    [Description("加盟商角色")]
    Join,

    /// <summary>
    /// 门店角色
    /// </summary>
    [Description("门店角色")]
    Store
}