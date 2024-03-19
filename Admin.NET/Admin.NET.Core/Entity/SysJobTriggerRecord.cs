// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 系统作业触发器运行记录表
/// </summary>
[SugarTable(null, "系统作业触发器运行记录表")]
[SysTable]
public partial class SysJobTriggerRecord : EntityBaseId
{
    /// <summary>
    /// 作业Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string JobId { get; set; }

    /// <summary>
    /// 触发器Id
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器Id", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string TriggerId { get; set; }

    /// <summary>
    /// 当前运行次数
    /// </summary>
    [SugarColumn(ColumnDescription = "当前运行次数")]
    public long NumberOfRuns { get; set; }

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
    /// 触发器状态
    /// </summary>
    [SugarColumn(ColumnDescription = "触发器状态")]
    public TriggerStatus Status { get; set; } = TriggerStatus.Ready;

    /// <summary>
    /// 本次执行结果
    /// </summary>
    [SugarColumn(ColumnDescription = "本次执行结果", Length = 128)]
    [MaxLength(128)]
    public string? Result { get; set; }

    /// <summary>
    /// 本次执行耗时
    /// </summary>
    [SugarColumn(ColumnDescription = "本次执行耗时")]
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间")]
    public DateTime? CreatedTime { get; set; }
}