namespace Admin.NET.Core.Service;

public interface ISysCacheService
{
    Task DelByParentKeyAsync(string key);

    Task DelByPatternAsync(string key);

    Task DelCacheKey(string cacheKey);

    Task<bool> ExistsAsync(string cacheKey);

    Task<List<string>> GetAllCacheKeys();

    Task<T> GetAsync<T>(string cacheKey);

    Task<int?> GetMaxDataScopeType(long userId);

    Task<List<long>> GetOrgIdList(long userId);

    Task<List<string>> GetPermission(long userId);

    Task<string> GetStringAsync(string cacheKey);

    Task RemoveAsync(string key);

    Task SetAsync(string cacheKey, object value);

    Task SetAsync(string cacheKey, object value, TimeSpan expire);

    Task SetMaxDataScopeType(long userId, int dataScopeType);

    Task SetOrgIdList(long userId, List<long> orgIdList);

    Task SetPermission(long userId, List<string> permissions);

    Task SetStringAsync(string cacheKey, string value);

    Task SetStringAsync(string cacheKey, string value, TimeSpan expire);
}