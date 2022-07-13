namespace Admin.NET.Core;

/// <summary>
/// 系统机构表
/// </summary>
[SugarTable("sys_org", "系统机构表")]
public class SysOrg : EntityTenant
{
    /// <summary>
    /// 父Id
    /// </summary>
    [SugarColumn(ColumnDescription = "父Id")]
    public long Pid { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 100)]
    [Required, MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "编码", Length = 50)]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Order { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 100)]
    [MaxLength(100)]
    public string Remark { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 级别 例如：省、市、县、区
    /// </summary>
    [SugarColumn(ColumnDescription = "级别")]
    [MaxLength(20)]
    public string Level { get; set; }

    /// <summary>
    /// 机构子项
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysOrg> Children { get; set; }
}