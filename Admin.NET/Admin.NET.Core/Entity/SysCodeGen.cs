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
/// 代码生成表
/// </summary>
[SugarTable(null, "代码生成表")]
[SysTable]
public class SysCodeGen : EntityBase
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
}