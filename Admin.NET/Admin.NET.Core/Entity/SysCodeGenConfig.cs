namespace Admin.NET.Core;

/// <summary>
/// 代码生成字段配置表
/// </summary>
[SugarTable("sys_code_gen_config", "代码生成字段配置表")]
public class SysCodeGenConfig : EntityBase
{
    /// <summary>
    /// 代码生成主表ID
    /// </summary>
    public long CodeGenId { get; set; }

    /// <summary>
    /// 数据库字段名
    /// </summary>
    [Required, MaxLength(100)]
    public string ColumnName { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    [MaxLength(100)]
    public string ColumnComment { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    [MaxLength(50)]
    public string NetType { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    [MaxLength(50)]
    public string EffectType { get; set; }

    /// <summary>
    /// 外键实体名称
    /// </summary>
    [MaxLength(50)]
    public string FkEntityName { get; set; }

    /// <summary>
    /// 外键表名称
    /// </summary>
    [MaxLength(100)]
    public string FkTableName { get; set; }

    /// <summary>
    /// 外键显示字段
    /// </summary>
    [MaxLength(50)]
    public string FkColumnName { get; set; }

    /// <summary>
    /// 外键显示字段.NET类型
    /// </summary>
    [MaxLength(50)]
    public string FkColumnNetType { get; set; }

    /// <summary>
    /// 字典code
    /// </summary>
    [MaxLength(50)]
    public string DictTypeCode { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    [MaxLength(5)]
    public string WhetherRetract { get; set; }

    /// <summary>
    /// 是否必填（字典）
    /// </summary>
    [MaxLength(5)]
    public string WhetherRequired { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    [MaxLength(5)]
    public string QueryWhether { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    [MaxLength(10)]
    public string QueryType { get; set; }

    /// <summary>
    /// 列表显示
    /// </summary>
    [MaxLength(5)]
    public string WhetherTable { get; set; }

    /// <summary>
    /// 增改
    /// </summary>
    [MaxLength(5)]
    public string WhetherAddUpdate { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    [MaxLength(5)]
    public string ColumnKey { get; set; }

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    [MaxLength(50)]
    public string DataType { get; set; }

    /// <summary>
    /// 是否通用字段
    /// </summary>
    [MaxLength(5)]
    public string WhetherCommon { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    public string DisplayColumn { get; set; }

    /// <summary>
    /// 选中值字段
    /// </summary>
    public string ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    public string PidColumn { get; set; }
}