namespace Admin.NET.Core;

/// <summary>
/// 系统用户机构表
/// </summary>
[SugarTable("sys_user_org", "系统用户机构表")]
[SqlSugarEntity]
public class SysUserOrg : EntityBaseId
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 机构
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public SysOrg SysOrg { get; set; }
}