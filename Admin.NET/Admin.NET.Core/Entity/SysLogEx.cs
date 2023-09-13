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