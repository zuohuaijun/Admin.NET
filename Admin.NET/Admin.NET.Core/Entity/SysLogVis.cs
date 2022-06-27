namespace Admin.NET.Core;

/// <summary>
/// 系统访问日志表
/// </summary>
[SugarTable("sys_log_vis", "系统访问日志表")]
public class SysLogVis : EntityBase
{
    /// <summary>
    /// 是否执行成功（Y-是，N-否）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否执行成功")]
    public YesNoEnum Success { get; set; }

    /// <summary>
    /// 具体消息
    /// </summary>
    [SugarColumn(ColumnDescription = "具体消息", ColumnDataType = "longtext,text,clob")]
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
    [SugarColumn(ColumnDescription = "地址", Length = 100)]
    [MaxLength(100)]
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
    /// 访问类型
    /// </summary>
    [SugarColumn(ColumnDescription = "访问类型")]
    public LoginTypeEnum VisType { get; set; }

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