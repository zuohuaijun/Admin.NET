// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 代码生成表
/// </summary>
[SugarTable(null, "代码生成表")]
[SysTable]
[SugarIndex("index_{table}_B", nameof(BusName), OrderByType.Asc)]
[SugarIndex("index_{table}_T", nameof(TableName), OrderByType.Asc)]
public partial class SysCodeGen : EntityBase
{
    /// <summary>
    /// 作者姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "作者姓名", Length = 32)]
    [MaxLength(32)]
    public string? AuthorName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    [SugarColumn(ColumnDescription = "是否移除表前缀", Length = 8)]
    [MaxLength(8)]
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 生成方式
    /// </summary>
    [SugarColumn(ColumnDescription = "生成方式", Length = 32)]
    [MaxLength(32)]
    public string? GenerateType { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [SugarColumn(ColumnDescription = "库定位器名", Length = 64)]
    [MaxLength(64)]
    public string? ConfigId { get; set; }

    /// <summary>
    /// 数据库名(保留字段)
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库库名", Length = 64)]
    [MaxLength(64)]
    public string? DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型", Length = 64)]
    [MaxLength(64)]
    public string? DbType { get; set; }

    /// <summary>
    /// 数据库链接
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库链接", Length = 256)]
    [MaxLength(256)]
    public string? ConnectionString { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库表名", Length = 128)]
    [MaxLength(128)]
    public string? TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    [SugarColumn(ColumnDescription = "命名空间", Length = 128)]
    [MaxLength(128)]
    public string? NameSpace { get; set; }

    /// <summary>
    /// 业务名
    /// </summary>
    [SugarColumn(ColumnDescription = "业务名", Length = 128)]
    [MaxLength(128)]
    public string? BusName { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单编码")]
    public long MenuPid { get; set; }

    /// <summary>
    /// 支持打印类型
    /// </summary>
    [SugarColumn(ColumnDescription = "支持打印类型", Length = 32)]
    [MaxLength(32)]
    public string? PrintType { get; set; }

    /// <summary>
    /// 打印模版名称
    /// </summary>
    [SugarColumn(ColumnDescription = "打印模版名称", Length = 32)]
    [MaxLength(32)]
    public string? PrintName { get; set; }
}