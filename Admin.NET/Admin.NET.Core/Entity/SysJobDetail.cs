// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 系统作业信息表
/// </summary>
[SugarTable(null, "系统作业信息表")]
[SysTable]
[SugarIndex("index_{table}_J", nameof(JobId), OrderByType.Asc)]
public class SysJobDetail : EntityBaseId
{
    /// <summary>
    /// 作业Id
    /// </summary>
    [SugarColumn(ColumnDescription = "作业Id", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string JobId { get; set; }

    /// <summary>
    /// 组名称
    /// </summary>
    [SugarColumn(ColumnDescription = "组名称", Length = 128)]
    [MaxLength(128)]
    public string? GroupName { get; set; } = "default";

    /// <summary>
    /// 作业类型FullName
    /// </summary>
    [SugarColumn(ColumnDescription = "作业类型", Length = 128)]
    [MaxLength(128)]
    public string? JobType { get; set; }

    /// <summary>
    /// 程序集Name
    /// </summary>
    [SugarColumn(ColumnDescription = "程序集", Length = 128)]
    [MaxLength(128)]
    public string? AssemblyName { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    /// 是否并行执行
    /// </summary>
    [SugarColumn(ColumnDescription = "是否并行执行")]
    public bool Concurrent { get; set; } = true;

    /// <summary>
    /// 是否扫描特性触发器
    /// </summary>
    [SugarColumn(ColumnDescription = "是否扫描特性触发器", ColumnName = "annotation")]
    public bool IncludeAnnotation { get; set; } = false;

    /// <summary>
    /// 额外数据
    /// </summary>
    [SugarColumn(ColumnDescription = "额外数据", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Properties { get; set; } = "{}";

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间")]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 作业创建类型
    /// </summary>
    [SugarColumn(ColumnDescription = "作业创建类型")]
    public JobCreateTypeEnum CreateType { get; set; } = JobCreateTypeEnum.BuiltIn;

    /// <summary>
    /// 脚本代码
    /// </summary>
    [SugarColumn(ColumnDescription = "脚本代码", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? ScriptCode { get; set; }
}