using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysCacheService
    {
        Task AddCacheKey(string cacheKey);

        Task DelByPatternAsync(string key);

        Task DelCacheKey(string cacheKey);

        bool Exists(string cacheKey);

        Task<List<string>> GetAllCacheKeys();

        Task<T> GetAsync<T>(string cacheKey);

        Task<List<long>> GetDataScope(long userId);

        Task<List<AntDesignTreeNode>> GetMenu(long userId, string appCode);

        Task<List<string>> GetPermission(long userId);

        Task<string> GetStringAsync(string cacheKey);

        Task RemoveAsync(string key);

        Task SetAsync(string cacheKey, object value);

        Task SetDataScope(long userId, List<long> dataScopes);

        Task SetMenu(long userId, string appCode, List<AntDesignTreeNode> menus);

        Task SetPermission(long userId, List<string> permissions);

        Task SetStringAsync(string cacheKey, string value);
    }
}