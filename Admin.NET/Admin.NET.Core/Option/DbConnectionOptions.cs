namespace Admin.NET.Core;

/// <summary>
/// 数据库链接配置
/// </summary>
public class DbConnectionOptions : IConfigurableOptions
{
    /// <summary>
    /// 启用初始化库表
    /// </summary>
    public bool EnableInitTable { get; set; }

    /// <summary>
    /// 启用库表差异日志
    /// </summary>
    public bool EnableDiffLog { get; set; }

    /// <summary>
    /// 数据库配置集合
    /// </summary>
    public List<ConnectionConfig> ConnectionConfigs { get; set; }
}