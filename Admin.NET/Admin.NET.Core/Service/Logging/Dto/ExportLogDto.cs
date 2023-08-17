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
/// 导出日志数据
/// </summary>
[ExcelExporter(Name = "日志数据", TableStyle = OfficeOpenXml.Table.TableStyles.None, AutoFitAllColumn = true)]
public class ExportLogDto
{
    /// <summary>
    /// 记录器类别名称
    /// </summary>
    [ExporterHeader(DisplayName = "记录器类别名称", IsBold = true)]
    public string LogName { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    [ExporterHeader(DisplayName = "日志级别", IsBold = true)]
    public string LogLevel { get; set; }

    /// <summary>
    /// 事件Id
    /// </summary>
    [ExporterHeader(DisplayName = "事件Id", IsBold = true)]
    public string EventId { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    [ExporterHeader(DisplayName = "日志消息", IsBold = true)]
    public string Message { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    [ExporterHeader(DisplayName = "异常对象", IsBold = true)]
    public string Exception { get; set; }

    /// <summary>
    /// 当前状态值
    /// </summary>
    [ExporterHeader(DisplayName = "当前状态值", IsBold = true)]
    public string State { get; set; }

    /// <summary>
    /// 日志记录时间
    /// </summary>
    [ExporterHeader(DisplayName = "日志记录时间", IsBold = true)]
    public DateTime LogDateTime { get; set; }

    /// <summary>
    /// 线程Id
    /// </summary>
    [ExporterHeader(DisplayName = "线程Id", IsBold = true)]
    public int ThreadId { get; set; }

    /// <summary>
    /// 请求跟踪Id
    /// </summary>
    [ExporterHeader(DisplayName = "请求跟踪Id", IsBold = true)]
    public string TraceId { get; set; }
}