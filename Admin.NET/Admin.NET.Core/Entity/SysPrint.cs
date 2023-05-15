namespace Admin.NET.Core;

/// <summary>
/// 系统打印模板表
/// </summary>
[SugarTable(null, "系统打印模板表")]
[SystemTable]
public class SysPrint : EntityTenant
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string Name { get; set; }

    /// <summary>
    /// 打印模板
    /// </summary>
    [SugarColumn(ColumnDescription = "打印模板", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    [Required]
    public virtual string Template { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int OrderNo { get; set; } = 100;

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 128)]
    [MaxLength(128)]
    public string? Remark { get; set; }
}