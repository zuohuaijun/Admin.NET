using System.Collections.Concurrent;

namespace Admin.NET.Core;

/// <summary>
/// 数据过滤器缓存
/// </summary>
public class SqlSugarQueryFilterCache : Singleton<SqlSugarQueryFilterCache>
{
    private ConcurrentDictionary<string, QueryFilterProvider> _queryEntityFilters = new();
    
    // 实体过滤器
    public ConcurrentDictionary<string, QueryFilterProvider> QueryEntityFilters => _queryEntityFilters;

    private SqlSugarQueryFilterCache()
    {
    }
}