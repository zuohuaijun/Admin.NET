// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 系统机构表
/// </summary>
[SugarTable(null, "系统机构表")]
[SysTable]
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
    [SugarColumn(ColumnDescription = "名称", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(ColumnDescription = "编码", Length = 64)]
    [MaxLength(64)]
    public string? Code { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    [SugarColumn(ColumnDescription = "级别")]
    public int? Level { get; set; }

    /// <summary>
    /// 机构类型-数据字典
    /// </summary>
    [SugarColumn(ColumnDescription = "机构类型", Length = 64)]
    [MaxLength(64)]
    public string? Type { get; set; }

    /// <summary>
    /// 负责人Id
    /// </summary>
    [SugarColumn(ColumnDescription = "负责人Id", IsNullable = true)]
    public long? DirectorId { get; set; }

    /// <summary>
    /// 负责人
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(DirectorId))]
    public SysUser Director { get; set; }

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

    /// <summary>
    /// 机构子项
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysOrg> Children { get; set; }

    /// <summary>
    /// 是否禁止选中
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public bool Disabled { get; set; }
}