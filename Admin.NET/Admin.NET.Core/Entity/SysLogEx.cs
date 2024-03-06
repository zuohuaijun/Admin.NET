// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 系统异常日志表
/// </summary>
[SugarTable(null, "系统异常日志表")]
[SysTable]
[LogTable]
public class SysLogEx : SysLogVis
{
    /// <summary>
    /// 请求方式
    /// </summary>
    [SugarColumn(ColumnDescription = "请求方式", Length = 32)]
    [MaxLength(32)]
    public string? HttpMethod { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    [SugarColumn(ColumnDescription = "请求地址", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? RequestParam { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    [SugarColumn(ColumnDescription = "返回结果", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? ReturnResult { get; set; }

    /// <summary>
    /// 事件Id
    /// </summary>
    [SugarColumn(ColumnDescription = "事件Id")]
    public int? EventId { get; set; }

    /// <summary>
    /// 线程Id
    /// </summary>
    [SugarColumn(ColumnDescription = "线程Id")]
    public int? ThreadId { get; set; }

    /// <summary>
    /// 请求跟踪Id
    /// </summary>
    [SugarColumn(ColumnDescription = "请求跟踪Id", Length = 128)]
    [MaxLength(128)]
    public string? TraceId { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    [SugarColumn(ColumnDescription = "异常信息", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Exception { get; set; }

    /// <summary>
    /// 日志消息Json
    /// </summary>
    [SugarColumn(ColumnDescription = "日志消息Json", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Message { get; set; }
}