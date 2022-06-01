namespace Admin.NET.Core;

/// <summary>
/// 系统租户表
/// </summary>
[SugarTable("sys_tenant", "系统租户表")]
[SqlSugarEntity]
public class SysTenant : EntityBase
{
    /// <summary>
    /// 公司名称
    /// </summary>
    [SugarColumn(ColumnDescription = "公司名称", Length = 30)]
    [Required, MaxLength(30)]
    public string Name { get; set; }

    /// <summary>
    /// 管理员名称
    /// </summary>
    [SugarColumn(ColumnDescription = "管理员名称", Length = 20)]
    [Required, MaxLength(20)]
    public string AdminName { get; set; }

    /// <summary>
    /// 主机
    /// </summary>
    [SugarColumn(ColumnDescription = "主机", Length = 100)]
    [MaxLength(100)]
    public string Host { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "电子邮箱", Length = 50)]
    [MaxLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [SugarColumn(ColumnDescription = "电话", Length = 20)]
    [MaxLength(20)]
    public string Phone { get; set; }

    /// <summary>
    /// 数据库连接
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库连接", Length = 200)]
    [MaxLength(200)]
    public string Connection { get; set; }

    /// <summary>
    /// 架构
    /// </summary>
    [SugarColumn(ColumnDescription = "架构", Length = 50)]
    [MaxLength(50)]
    public string Schema { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 100)]
    [MaxLength(100)]
    public string Remark { get; set; }
}