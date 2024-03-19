// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 系统差异日志表
/// </summary>
[SugarTable(null, "系统差异日志表")]
[SysTable]
[LogTable]
public partial class SysLogDiff : EntityBase
{
    /// <summary>
    /// 操作前记录
    /// </summary>
    [SugarColumn(ColumnDescription = "操作前记录", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? BeforeData { get; set; }

    /// <summary>
    /// 操作后记录
    /// </summary>
    [SugarColumn(ColumnDescription = "操作后记录", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? AfterData { get; set; }

    /// <summary>
    /// Sql
    /// </summary>
    [SugarColumn(ColumnDescription = "Sql", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Sql { get; set; }

    /// <summary>
    /// 参数  手动传入的参数
    /// </summary>
    [SugarColumn(ColumnDescription = "参数", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Parameters { get; set; }

    /// <summary>
    /// 业务对象
    /// </summary>
    [SugarColumn(ColumnDescription = "业务对象", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? BusinessData { get; set; }

    /// <summary>
    /// 差异操作
    /// </summary>
    [SugarColumn(ColumnDescription = "差异操作", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? DiffType { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    [SugarColumn(ColumnDescription = "耗时")]
    public long? Elapsed { get; set; }
}