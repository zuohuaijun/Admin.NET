namespace Admin.NET.Core;

/// <summary>
/// 系统在线用户表
/// </summary>
[SugarTable("sys_online_user", "系统在线用户表")]
public class SysOnlineUser : EntityBaseId
{
    /// <summary>
    /// 连接Id
    /// </summary>
    [SugarColumn(ColumnDescription = "连接Id")]
    public string ConnectionId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号", Length = 32)]
    [Required, MaxLength(32)]
    public string Account { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "姓名", Length = 32)]
    [MaxLength(32)]
    public string Name { get; set; }

    /// <summary>
    /// 最后连接时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最后连接时间")]
    public DateTime LastTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录IP", Length = 16)]
    [MaxLength(16)]
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录浏览器
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录浏览器", Length = 32)]
    [MaxLength(32)]
    public string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录所用系统
    /// </summary>
    [SugarColumn(ColumnDescription = "最后登录系统", Length = 32)]
    [MaxLength(32)]
    public string LastLoginOs { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id")]
    public long TenantId { get; set; }
}