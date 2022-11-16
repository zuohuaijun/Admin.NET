namespace Admin.NET.Core.Service;

/// <summary>
/// 数据库
/// </summary>
public class DatabaseOutput
{
    /// <summary>
    /// 库定位器名
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public SqlSugar.DbType DbType { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}