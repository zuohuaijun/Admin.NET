namespace Admin.NET.Core;

/// <summary>
/// 系统异常日志表
/// </summary>
[SugarTable("sys_log_ex", "系统异常日志表")]
public class SysLogEx : EntityBase
{
    /// <summary>
    /// 日志名称
    /// </summary>
    [SugarColumn(ColumnDescription = "日志名称", Length = 200)]
    public string LogName { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    [SugarColumn(ColumnDescription = "日志级别", Length = 20)]
    public string LogLevel { get; set; }

    /// <summary>
    /// 事件Id
    /// </summary>
    [SugarColumn(ColumnDescription = "事件Id", ColumnDataType = "longtext,text,clob")]
    public string EventId { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    [SugarColumn(ColumnDescription = "日志消息", ColumnDataType = "longtext,text,clob")]
    public string Message { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    [SugarColumn(ColumnDescription = "异常对象", ColumnDataType = "longtext,text,clob")]
    public string Exception { get; set; }
}