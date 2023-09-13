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
/// 系统作业触发器表
/// </summary>
[SugarTable(null, "系统作业触发器表")]
[SysTable]
public class SysJobTrigger : EntityBaseId
{
    /// <summary>
    /// 触发器Id
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器Id", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string TriggerId { get; set; }

    /// <summary>
    /// 作业Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string JobId { get; set; }

    /// <summary>
    /// 触发器类型FullName
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器类型", Length = 128)]
    [MaxLength(128)]
    public string? TriggerType { get; set; }

    /// <summary>
    /// 程序集Name
    /// </summary>
    [SugarColumn(ColumnDescription = "程序集", Length = 128)]
    [MaxLength(128)]
    public string? AssemblyName { get; set; } = "Furion.Pure";

    /// <summary>
    /// 参数
    /// </summary>
    [SugarColumn(ColumnDescription = "参数", Length = 128)]
    [MaxLength(128)]
    public string? Args { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public TriggerStatus Status { get; set; } = TriggerStatus.Ready;

    /// <summary>
    /// 起始时间
    /// </summary>
    [SugarColumn(ColumnDescription = "起始时间")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnDescription = "结束时间")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 最近运行时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最近运行时间")]
    public DateTime? LastRunTime { get; set; }

    /// <summary>
    /// 下一次运行时间
    /// </summary>
    [SugarColumn(ColumnDescription = "下一次运行时间")]
    public DateTime? NextRunTime { get; set; }

    /// <summary>
    /// 触发次数
    /// </summary>
    [SugarColumn(ColumnDescription = "触发次数")]
    public long NumberOfRuns { get; set; }

    /// <summary>
    /// 最大触发次数（0:不限制，n:N次）
    /// </summary>
    [SugarColumn(ColumnDescription = "最大触发次数")]
    public long MaxNumberOfRuns { get; set; }

    /// <summary>
    /// 出错次数
    /// </summary>
    [SugarColumn(ColumnDescription = "出错次数")]
    public long NumberOfErrors { get; set; }

    /// <summary>
    /// 最大出错次数（0:不限制，n:N次）
    /// </summary>
    [SugarColumn(ColumnDescription = "最大出错次数")]
    public long MaxNumberOfErrors { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    [SugarColumn(ColumnDescription = "重试次数")]
    public int NumRetries { get; set; }

    /// <summary>
    /// 重试间隔时间（ms）
    /// </summary>
    [SugarColumn(ColumnDescription = "重试间隔时间(ms)")]
    public int RetryTimeout { get; set; } = 1000;

    /// <summary>
    /// 是否立即启动
    /// </summary>
    [SugarColumn(ColumnDescription = "是否立即启动")]
    public bool StartNow { get; set; } = true;

    /// <summary>
    /// 是否启动时执行一次
    /// </summary>
    [SugarColumn(ColumnDescription = "是否启动时执行一次")]
    public bool RunOnStart { get; set; } = false;

    /// <summary>
    /// 是否在启动时重置最大触发次数等于一次的作业
    /// </summary>
    [SugarColumn(ColumnDescription = "是否重置触发次数")]
    public bool ResetOnlyOnce { get; set; } = true;

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间")]
    public DateTime? UpdatedTime { get; set; }
}