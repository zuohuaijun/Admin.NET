using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysCacheService
    {
        bool Del(string key);
        Task<bool> DelAsync(string key);
        Task<bool> DelByPatternAsync(string key);
        List<string> GetAllCacheKeys();
        Task<List<long>> GetDataScope(long userId);
        Task<List<AntDesignTreeNode>> GetMenu(long userId, string appCode);
        Task<List<string>> GetPermission(long userId);
        Task SetDataScope(long userId, List<long> dataScopes);
        Task SetMenu(long userId, string appCode, List<AntDesignTreeNode> menus);
        Task SetPermission(long userId, List<string> permissions);
        bool Set(string key, object value);
        Task<bool> SetAsync(string key, object value);
        string Get(string key);
        Task<string> GetAsync(string key);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
    }
}