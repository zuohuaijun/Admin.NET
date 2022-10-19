namespace Admin.NET.Core;

/// <summary>
/// 系统租户表
/// </summary>
[SugarTable("sys_tenant", "系统租户表")]
public class SysTenant : EntityBase
{
    /// <summary>
    /// 租户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "租户名称", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string Name { get; set; }

    /// <summary>
    /// 管理员名称
    /// </summary>
    [SugarColumn(ColumnDescription = "管理员名称", Length = 32)]
    [Required, MaxLength(32)]
    public virtual string AdminName { get; set; }

    /// <summary>
    /// 主机
    /// </summary>
    [SugarColumn(ColumnDescription = "主机", Length = 128)]
    [MaxLength(128)]
    public string Host { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "电子邮箱", Length = 64)]
    [MaxLength(64)]
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [SugarColumn(ColumnDescription = "电话", Length = 16)]
    [MaxLength(16)]
    public string Phone { get; set; }

    /// <summary>
    /// 数据库连接
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库连接", Length = 256)]
    [MaxLength(256)]
    public string Connection { get; set; }

    /// <summary>
    /// 架构
    /// </summary>
    [SugarColumn(ColumnDescription = "架构", Length = 64)]
    [MaxLength(64)]
    public string Schema { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Order { get; set; } = 100;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 128)]
    [MaxLength(128)]
    public string Remark { get; set; }
}