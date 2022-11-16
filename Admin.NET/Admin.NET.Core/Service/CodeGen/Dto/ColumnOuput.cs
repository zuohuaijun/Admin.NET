namespace Admin.NET.Core.Service;

/// <summary>
/// 数据库表列
/// </summary>
public class ColumnOuput
{
    /// <summary>
    /// 字段名
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 数据库中类型
    /// </summary>
    public string DataType { get; set; }

    /// <summary>
    /// .NET字段类型
    /// </summary>
    public string NetType { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    public string ColumnComment { get; set; }

    /// <summary>
    /// 主外键
    /// </summary>
    public string ColumnKey { get; set; }
}