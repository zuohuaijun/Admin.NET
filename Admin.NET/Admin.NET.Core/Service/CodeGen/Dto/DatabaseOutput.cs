namespace Admin.NET.Core.Service;

/// <summary>
/// 多库代码生成库列表输出
/// </summary>
public class DatabaseOutput
{
    /// <summary>
    /// 库定位器名
    /// </summary>
    public dynamic DbConfigId { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}