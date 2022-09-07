namespace Admin.NET.Core;

/// <summary>
/// ElasticSearch配置选项
/// </summary>
public sealed class ElasticSearchOptions : IConfigurableOptions
{
    /// <summary>
    /// ES地址集合
    /// </summary>
    public List<string> ServerUris { get; set; }

    /// <summary>
    /// 默认索引
    /// </summary>
    public string DefaultIndex { get; set; }
}