namespace Admin.NET.Core;

/// <summary>
/// 系统字典类型表
/// </summary>
[SugarTable("sys_dict_type", "系统字典类型表")]
public class SysDictType : EntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 50)]
    [Required, MaxLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "编码", Length = 50)]
    [Required, MaxLength(50)]
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
}