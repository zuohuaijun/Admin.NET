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
/// 系统审计日志表
/// </summary>
[SugarTable(null, "系统审计日志表")]
[SysTable]
[LogTable]
public class SysLogAudit : EntityBase
{
    /// <summary>
    /// 表名
    /// </summary>
    [SugarColumn(ColumnDescription = "表名", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string TableName { get; set; }

    /// <summary>
    /// 列名
    /// </summary>
    [SugarColumn(ColumnDescription = "列名", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string ColumnName { get; set; }

    /// <summary>
    /// 新值
    /// </summary>
    [SugarColumn(ColumnDescription = "新值", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? NewValue { get; set; }

    /// <summary>
    /// 旧值
    /// </summary>
    [SugarColumn(ColumnDescription = "旧值", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? OldValue { get; set; }

    /// <summary>
    /// 操作方式（新增、更新、删除）
    /// </summary>
    [SugarColumn(ColumnDescription = "操作方式")]
    public DataOpTypeEnum Operate { get; set; }

    /// <summary>
    /// 审计时间
    /// </summary>
    [SugarColumn(ColumnDescription = "审计时间")]
    public DateTime? AuditTime { get; set; }
}