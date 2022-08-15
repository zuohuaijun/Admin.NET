namespace Admin.NET.Core.Service;

public interface ISysCacheService
{
    Task AddCacheKey(string cacheKey);

    Task DelByPatternAsync(string key);

    Task DelCacheKey(string cacheKey);

    Task<bool> ExistsAsync(string cacheKey);

    Task<List<string>> GetAllCacheKeys();

    Task<T> GetAsync<T>(string cacheKey);

    Task<string> GetStringAsync(string cacheKey);

    Task RemoveAsync(string key);

    Task SetAsync(string cacheKey, object value);

    Task SetAsync(string cacheKey, object value, TimeSpan expire);

    Task SetStringAsync(string cacheKey, string value);

    Task SetStringAsync(string cacheKey, string value, TimeSpan expire);

    Task<List<long>> GetOrgIdList(long userId);

    Task SetOrgIdList(long userId, List<long> orgIdList);

    Task<List<string>> GetPermission(long userId);

    Task SetPermission(long userId, List<string> permissions);

    Task<int?> GetMaxDataScopeType(long userId);

    Task SetMaxDataScopeType(long userId, int dataScopeType);

    Task DelByParentKeyAsync(string key);
}