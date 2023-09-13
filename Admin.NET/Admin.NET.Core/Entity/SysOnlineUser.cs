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
/// 系统在线用户表
/// </summary>
[SugarTable(null, "系统在线用户表")]
[SysTable]
public class SysOnlineUser : EntityTenantId
{
    /// <summary>
    /// 连接Id
    /// </summary>
    [SugarColumn(ColumnDescription = "连接Id")]
    public string? ConnectionId { get; set; }

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
    public virtual string UserName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 32)]
    [MaxLength(32)]
    public string? RealName { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    [SugarColumn(ColumnDescription = "连接时间")]
    public DateTime? Time { get; set; }

    /// <summary>
    /// 连接IP
    /// </summary>
    [SugarColumn(ColumnDescription = "连接IP", Length = 256)]
    [MaxLength(256)]
    public string? Ip { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [SugarColumn(ColumnDescription = "浏览器", Length = 128)]
    [MaxLength(128)]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnDescription = "操作系统", Length = 128)]
    [MaxLength(128)]
    public string? Os { get; set; }
}