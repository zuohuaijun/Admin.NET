namespace Admin.NET.Core;

/// <summary>
/// 系统操作日志表
/// </summary>
[SugarTable("sys_log_op", "系统操作日志表")]
public class SysLogOp : EntityBase
{
    /// <summary>
    /// 是否执行成功（Y-是，N-否）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否执行成功")]
    public YesNoEnum Success { get; set; }

    /// <summary>
    /// 具体消息
    /// </summary>
    [SugarColumn(ColumnDescription = "具体消息", ColumnDataType = "text")]
    public string Message { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    [SugarColumn(ColumnDescription = "IP地址", Length = 20)]
    [MaxLength(20)]
    public string Ip { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [SugarColumn(ColumnDescription = "地址", Length = 300)]
    [MaxLength(300)]
    public string Location { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器", Length = 100)]
    [MaxLength(100)]
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnDescription = "操作系统", Length = 100)]
    [MaxLength(100)]
    public string Os { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    [SugarColumn(ColumnDescription = "请求地址", Length = 100)]
    [MaxLength(100)]
    public string Url { get; set; }

    /// <summary>
    /// 类名称
    /// </summary>
    [SugarColumn(ColumnDescription = "类名称", Length = 100)]
    [MaxLength(100)]
    public string ClassName { get; set; }

    /// <summary>
    /// 方法名称
    /// </summary>
    [SugarColumn(ColumnDescription = "方法名称", Length = 100)]
    [MaxLength(100)]
    public string MethodName { get; set; }

    /// <summary>
    /// 请求方式（GET POST PUT DELETE)
    /// </summary>
    [SugarColumn(ColumnDescription = "请求方式", Length = 10)]
    [MaxLength(10)]
    public string ReqMethod { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数", ColumnDataType = "text")]
    public string Param { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    [SugarColumn(ColumnDescription = "返回结果", ColumnDataType = "text")]
    public string Result { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "耗时")]
    public long ElapsedTime { get; set; }

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