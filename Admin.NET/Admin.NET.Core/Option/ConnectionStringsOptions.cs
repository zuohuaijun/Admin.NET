namespace Admin.NET.Core;

/// <summary>
/// 数据库链接配置
/// </summary>
public class ConnectionStringsOptions : IConfigurableOptions
{
    /// <summary>
    /// 默认数据库标识
    /// </summary>
    public string DefaultConfigId { get; set; } = SqlSugarConst.ConfigId;

    /// <summary>
    /// 默认数据库类型
    /// </summary>
    public string DefaultDbType { get; set; }

    /// <summary>
    /// 默认数据库连接字符串
    /// </summary>

    public string DefaultConnection { get; set; }

    /// <summary>
    /// 启用初始化库表
    /// </summary>
    public bool EnableInitTable { get; set; }

    /// <summary>
    /// 启用种子数据
    /// </summary>
    public bool EnableSeedData { get; set; }

    /// <summary>
    /// 启用库表差异日志
    /// </summary>
    public bool EnableDiffLog { get; set; }

    /// <summary>
    /// 业务库集合
    /// </summary>
    public List<DbConfig> DbConfigs { get; set; } = new List<DbConfig>();
}

/// <summary>
/// 数据库参数
/// </summary>
public class DbConfig
{
    /// <summary>
    /// 数据库编号
    /// </summary>
    public string DbConfigId { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string DbConnection { get; set; }

    /// <summary>
    /// 启用初始化库表
    /// </summary>
    public bool EnableInitTable { get; set; }

    /// <summary>
    /// 启用种子数据
    /// </summary>
    public bool EnableSeedData { get; set; }
}