namespace Admin.NET.Core;

/// <summary>
/// SqlSugar实体配置特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Class)]
public class SqlSugarEntityAttribute : Attribute
{
    /// <summary>
    /// 数据库上下文标识
    /// </summary>
    public string DbConfigId { get; set; } = SqlSugarConst.ConfigId;

    /// <summary>
    /// 实体顺序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 默认配置
    /// </summary>
    public SqlSugarEntityAttribute()
    {
    }

    /// <summary>
    /// 配置实体排序
    /// </summary>
    /// <param name="order"></param>
    public SqlSugarEntityAttribute(int order)
    {
        Order = order;
    }

    /// <summary>
    /// 配置数据库标识和实体排序
    /// </summary>
    /// <param name="order"></param>
    /// <param name="dbConfigId"></param>
    public SqlSugarEntityAttribute(int order, string dbConfigId)
    {
        Order = order;
        DbConfigId = dbConfigId;
    }
}