// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 数据库配置选项
/// </summary>
public sealed class DbConnectionOptions : IConfigurableOptions<DbConnectionOptions>
{
    /// <summary>
    /// 启用控制台打印SQL
    /// </summary>
    public bool EnableConsoleSql { get; set; }

    /// <summary>
    /// 数据库集合
    /// </summary>
    public List<DbConnectionConfig> ConnectionConfigs { get; set; }

    public void PostConfigure(DbConnectionOptions options, IConfiguration configuration)
    {
        foreach (var dbConfig in options.ConnectionConfigs)
        {
            if (dbConfig.ConfigId == null || string.IsNullOrWhiteSpace(dbConfig.ConfigId.ToString()))
                dbConfig.ConfigId = SqlSugarConst.MainConfigId;
        }
    }
}

/// <summary>
/// 数据库连接配置
/// </summary>
public sealed class DbConnectionConfig : ConnectionConfig
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public DbSettings DbSettings { get; set; }

    /// <summary>
    /// 表配置
    /// </summary>
    public TableSettings TableSettings { get; set; }

    /// <summary>
    /// 种子配置
    /// </summary>
    public SeedSettings SeedSettings { get; set; }
}

/// <summary>
/// 数据库配置
/// </summary>
public sealed class DbSettings
{
    /// <summary>
    /// 启用库表初始化
    /// </summary>
    public bool EnableInitDb { get; set; }

    /// <summary>
    /// 启用库表差异日志
    /// </summary>
    public bool EnableDiffLog { get; set; }

    /// <summary>
    /// 启用驼峰转下划线
    /// </summary>
    public bool EnableUnderLine { get; set; }
}

/// <summary>
/// 表配置
/// </summary>
public sealed class TableSettings
{
    /// <summary>
    /// 启用表初始化
    /// </summary>
    public bool EnableInitTable { get; set; }

    /// <summary>
    /// 启用表增量更新
    /// </summary>
    public bool EnableIncreTable { get; set; }
}

/// <summary>
/// 种子配置
/// </summary>
public sealed class SeedSettings
{
    /// <summary>
    /// 启用种子初始化
    /// </summary>
    public bool EnableInitSeed { get; set; }

    /// <summary>
    /// 启用种子增量更新
    /// </summary>
    public bool EnableIncreSeed { get; set; }
}