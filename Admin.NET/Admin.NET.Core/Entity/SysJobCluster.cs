namespace Admin.NET.Core;

/// <summary>
/// 系统作业集群表
/// </summary>
[SugarTable("sys_job_cluster", "系统作业集群表")]
public class SysJobCluster : BaseId
{
    /// <summary>
    /// 作业集群Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业集群Id", Length = 64)]
    [MaxLength(64)]
    public virtual string ClusterId { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
    [MaxLength(128)]
    public string Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public ClusterStatus Status { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间")]
    public DateTime? UpdatedTime { get; set; }
}