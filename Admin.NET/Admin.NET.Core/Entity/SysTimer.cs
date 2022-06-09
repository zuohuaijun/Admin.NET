namespace Admin.NET.Core;

/// <summary>
/// 定时任务
/// </summary>
[SugarTable("sys_timer", "定时任务表")]
public class SysTimer : EntityBase
{
    /// <summary>
    /// 任务名称
    /// </summary>
    [SugarColumn(ColumnDescription = "任务名称", Length = 20)]
    [Required, MaxLength(20)]
    public virtual string TimerName { get; set; }

    /// <summary>
    /// 是否只执行一次
    /// </summary>
    [SugarColumn(ColumnDescription = "是否只执行一次")]
    public bool DoOnce { get; set; } = false;

    /// <summary>
    /// 是否立即执行
    /// </summary>
    [SugarColumn(ColumnDescription = "是否立即执行")]
    public bool StartNow { get; set; } = false;

    /// <summary>
    /// 执行类型(串行并行)
    /// </summary>
    [SugarColumn(ColumnDescription = "执行类型")]
    public SpareTimeExecuteTypes ExecuteType { get; set; } = SpareTimeExecuteTypes.Parallel;

    /// <summary>
    /// 执行间隔（单位秒）
    /// </summary>
    /// <example>5</example>
    [SugarColumn(ColumnDescription = "执行间隔")]
    public int? Interval { get; set; } = 5;

    /// <summary>
    /// Cron表达式
    /// </summary>
    [SugarColumn(ColumnDescription = "Cron表达式", Length = 20)]
    [MaxLength(20)]
    public string Cron { get; set; }

    /// <summary>
    /// 定时器类型
    /// </summary>
    [SugarColumn(ColumnDescription = "定时器类型")]
    public SpareTimeTypes TimerType { get; set; } = SpareTimeTypes.Interval;

    /// <summary>
    /// 请求url
    /// </summary>
    [SugarColumn(ColumnDescription = "请求url", Length = 200)]
    [MaxLength(200)]
    public string RequestUrl { get; set; }

    /// <summary>
    /// 请求类型
    /// </summary>
    [SugarColumn(ColumnDescription = "请求类型")]
    public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.Post;

    /// <summary>
    /// 请求参数
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数")]
    public string RequestPara { get; set; }

    /// <summary>
    /// Headers参数 比如{"Authorization":"userpassword"}
    /// </summary>
    [SugarColumn(ColumnDescription = "Headers参数")]
    public string Headers { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 100)]
    [MaxLength(100)]
    public string Remark { get; set; }
}