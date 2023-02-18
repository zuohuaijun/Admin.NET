namespace Admin.NET.Core;

/// <summary>
/// 系统访问日志表
/// </summary>
[SugarTable(null, "系统访问日志表")]
[SystemTable]
public class SysLogVis : EntityTenant
{
    /// <summary>
    /// 模块名称
    /// </summary>
    [SugarColumn(ColumnDescription = "模块名称", Length = 256)]
    [MaxLength(256)]
    public string? ControllerName { get; set; }

    /// <summary>
    /// 方法名称
    ///</summary>
    [SugarColumn(ColumnDescription = "方法名称", Length = 256)]
    [MaxLength(256)]
    public string? ActionName { get; set; }

    /// <summary>
    /// 显示名称
    ///</summary>
    [SugarColumn(ColumnDescription = "显示名称", Length = 256)]
    [MaxLength(256)]
    public string? DisplayTitle { get; set; }

    /// <summary>
    /// 执行状态
    /// </summary>
    [SugarColumn(ColumnDescription = "执行状态", Length = 32)]
    [MaxLength(32)]
    public string? Status { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    [SugarColumn(ColumnDescription = "IP地址", Length = 256)]
    [MaxLength(256)]
    public string? RemoteIp { get; set; }

    /// <summary>
    /// 登录地点
    /// </summary>
    [SugarColumn(ColumnDescription = "登录地点", Length = 128)]
    [MaxLength(128)]
    public string? Location { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    [SugarColumn(ColumnDescription = "经度")]
    public double? Longitude { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    [SugarColumn(ColumnDescription = "维度")]
    public double? Latitude { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器", Length = 256)]
    [MaxLength(256)]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnDescription = "操作系统", Length = 256)]
    [MaxLength(256)]
    public string? Os { get; set; }

    /// <summary>
    /// 操作用时
    /// </summary>
    [SugarColumn(ColumnDescription = "操作用时")]
    public long? Elapsed { get; set; }

    /// <summary>
    /// 日志时间
    /// </summary>
    [SugarColumn(ColumnDescription = "日志时间")]
    public DateTime? LogDateTime { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号", Length = 32)]
    [MaxLength(32)]
    public string? Account { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 32)]
    [MaxLength(32)]
    public string? RealName { get; set; }
}