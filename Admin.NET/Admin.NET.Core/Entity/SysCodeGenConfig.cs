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
/// 代码生成字段配置表
/// </summary>
[SugarTable(null, "代码生成字段配置表")]
[SysTable]
public class SysCodeGenConfig : EntityBase
{
    /// <summary>
    /// 代码生成主表Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主表Id")]
    public long CodeGenId { get; set; }

    /// <summary>
    /// 数据库字段名
    /// </summary>
    [SugarColumn(ColumnDescription = "字段名称", Length = 128)]
    [Required, MaxLength(128)]
    public virtual string ColumnName { get; set; }

    /// <summary>
    /// 实体属性名
    /// </summary>
    [SugarColumn(ColumnDescription = "属性名称", Length = 128)]
    [Required, MaxLength(128)]
    public virtual string PropertyName { get; set; }

    /// <summary>
    /// 字段数据长度
    /// </summary>
    [SugarColumn(ColumnDescription = "字段数据长度", DefaultValue = "0")]
    public int ColumnLength { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    [SugarColumn(ColumnDescription = "字段描述", Length = 128)]
    [MaxLength(128)]
    public string? ColumnComment { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    [SugarColumn(ColumnDescription = "NET数据类型", Length = 64)]
    [MaxLength(64)]
    public string? NetType { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "作用类型", Length = 64)]
    [MaxLength(64)]
    public string? EffectType { get; set; }

    /// <summary>
    /// 外键实体名称
    /// </summary>
    [SugarColumn(ColumnDescription = "外键实体名称", Length = 64)]
    [MaxLength(64)]
    public string? FkEntityName { get; set; }

    /// <summary>
    /// 外键表名称
    /// </summary>
    [SugarColumn(ColumnDescription = "外键表名称", Length = 128)]
    [MaxLength(128)]
    public string? FkTableName { get; set; }

    /// <summary>
    /// 外键显示字段
    /// </summary>
    [SugarColumn(ColumnDescription = "外键显示字段", Length = 64)]
    [MaxLength(64)]
    public string? FkColumnName { get; set; }

    /// <summary>
    /// 外键显示字段.NET类型
    /// </summary>
    [SugarColumn(ColumnDescription = "外键显示字段.NET类型", Length = 64)]
    [MaxLength(64)]
    public string? FkColumnNetType { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    [SugarColumn(ColumnDescription = "字典编码", Length = 64)]
    [MaxLength(64)]
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "列表是否缩进", Length = 8)]
    [MaxLength(8)]
    public string? WhetherRetract { get; set; }

    /// <summary>
    /// 是否必填（字典）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否必填", Length = 8)]
    [MaxLength(8)]
    public string? WhetherRequired { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    [SugarColumn(ColumnDescription = "是否是查询条件", Length = 8)]
    [MaxLength(8)]
    public string? QueryWhether { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    [SugarColumn(ColumnDescription = "查询方式", Length = 16)]
    [MaxLength(16)]
    public string? QueryType { get; set; }

    /// <summary>
    /// 列表显示
    /// </summary>
    [SugarColumn(ColumnDescription = "列表显示", Length = 8)]
    [MaxLength(8)]
    public string? WhetherTable { get; set; }

    /// <summary>
    /// 增改
    /// </summary>
    [SugarColumn(ColumnDescription = "增改", Length = 8)]
    [MaxLength(8)]
    public string? WhetherAddUpdate { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnDescription = "主键", Length = 8)]
    [MaxLength(8)]
    public string? ColumnKey { get; set; }

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库中类型", Length = 64)]
    [MaxLength(64)]
    public string? DataType { get; set; }

    /// <summary>
    /// 是否通用字段
    /// </summary>
    [SugarColumn(ColumnDescription = "是否通用字段", Length = 8)]
    [MaxLength(8)]
    public string? WhetherCommon { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    [SugarColumn(ColumnDescription = "显示文本字段", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? DisplayColumn { get; set; }

    /// <summary>
    /// 选中值字段
    /// </summary>
    [SugarColumn(ColumnDescription = "选中值字段", Length = 128)]
    [MaxLength(128)]
    public string? ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    [SugarColumn(ColumnDescription = "父级字段", Length = 128)]
    [MaxLength(128)]
    public string? PidColumn { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int OrderNo { get; set; } = 100;
}