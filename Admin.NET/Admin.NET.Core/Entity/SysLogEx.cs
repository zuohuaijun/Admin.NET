namespace Admin.NET.Core;

/// <summary>
/// 系统异常日志表
/// </summary>
[SugarTable("sys_log_ex", "系统异常日志表")]
[SqlSugarEntity]
public class SysLogEx : EntityBase
{
    /// <summary>
    /// 类名
    /// </summary>
    [SugarColumn(ColumnDescription = "类名", Length = 100)]
    [MaxLength(100)]
    public string ClassName { get; set; }

    /// <summary>
    /// 方法名
    /// </summary>
    [SugarColumn(ColumnDescription = "方法名", Length = 100)]
    [MaxLength(100)]
    public string MethodName { get; set; }

    /// <summary>
    /// 异常名称
    /// </summary>
    [SugarColumn(ColumnDescription = "异常名称", ColumnDataType = "text")]
    public string ExceptionName { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    [SugarColumn(ColumnDescription = "异常信息", ColumnDataType = "text")]
    public string ExceptionMsg { get; set; }

    /// <summary>
    /// 异常源
    /// </summary>
    [SugarColumn(ColumnDescription = "异常源", ColumnDataType = "text")]
    public string ExceptionSource { get; set; }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    [SugarColumn(ColumnDescription = "堆栈信息", ColumnDataType = "text")]
    public string StackTrace { get; set; }

    /// <summary>
    /// 参数对象
    /// </summary>
    [SugarColumn(ColumnDescription = "参数对象", ColumnDataType = "text")]
    public string ParamsObj { get; set; }

    /// <summary>
    /// 账号名称
    /// </summary>
    [SugarColumn(ColumnDescription = "账号名称", Length = 20)]
    [MaxLength(20)]
    public string UserName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 20)]
    [MaxLength(20)]
    public string RealName { get; set; }
}