using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统缓存服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Cache", Order = 100)]
    public class SysCacheService : ISysCacheService, IDynamicApiController, ISingleton
    {
        private readonly IDistributedCache _cache;

        public SysCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取数据范围缓存（机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetDataScope(long userId)
        {
            var cacheKey = CommonConst.CACHE_KEY_DATASCOPE + $"{userId}";
            var res = await _cache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<long>>(res);
        }

        /// <summary>
        /// 缓存数据范围（机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dataScopes"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SetDataScope(long userId, List<long> dataScopes)
        {
            var cacheKey = CommonConst.CACHE_KEY_DATASCOPE + $"{userId}";
            await _cache.SetStringAsync(cacheKey, JSON.Serialize(dataScopes));

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 获取菜单缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<AntDesignTreeNode>> GetMenu(long userId, string appCode)
        {
            var cacheKey = CommonConst.CACHE_KEY_MENU + $"{userId}-{appCode}";
            var res = await _cache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<AntDesignTreeNode>>(res);
        }

        /// <summary>
        /// 缓存菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appCode"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SetMenu(long userId, string appCode, List<AntDesignTreeNode> menus)
        {
            var cacheKey = CommonConst.CACHE_KEY_MENU + $"{userId}-{appCode}";
            await _cache.SetStringAsync(cacheKey, JSON.Serialize(menus));

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 获取权限缓存（按钮）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<string>> GetPermission(long userId)
        {
            var cacheKey = CommonConst.CACHE_KEY_PERMISSION + $"{userId}";
            var res = await _cache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<string>>(res);
        }

        /// <summary>
        /// 缓存权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SetPermission(long userId, List<string> permissions)
        {
            var cacheKey = CommonConst.CACHE_KEY_PERMISSION + $"{userId}";
            await _cache.SetStringAsync(cacheKey, JSON.Serialize(permissions));

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 获取所有缓存关键字
        /// </summary>
        /// <returns></returns>
        [HttpGet("sysCache/keyList")]
        public async Task<List<string>> GetAllCacheKeys()
        {
            var res = await _cache.GetStringAsync(CommonConst.CACHE_KEY_ALL);
            return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<string>>(res);
        }

        /// <summary>
        /// 删除指定关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("sysCache/remove")]
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);

            await DelCacheKey(key);
        }

        /// <summary>
        /// 删除某特征关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public async Task DelByPatternAsync(string key)
        {
            var allkeys = await GetAllCacheKeys();
            var delAllkeys = allkeys.Where(u => u.Contains(key)).ToList();

            // 删除相应的缓存
            delAllkeys.ForEach(u =>
            {
                _cache.Remove(u);
            });

            // 更新所有缓存键
            allkeys = allkeys.Where(u => !u.Contains(key)).ToList();
            await _cache.SetStringAsync(CommonConst.CACHE_KEY_ALL, JSON.Serialize(allkeys));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SetAsync(string cacheKey, object value)
        {
            await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JSON.Serialize(value)));

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SetStringAsync(string cacheKey, string value)
        {
            await _cache.SetStringAsync(cacheKey, value);

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpGet("sysCache/detail")]
        public async Task<string> GetStringAsync(string cacheKey)
        {
            return await _cache.GetStringAsync(cacheKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var res = await _cache.GetAsync(cacheKey);
            return res == null ? default : JSON.Deserialize<T>(Encoding.UTF8.GetString(res));
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        [NonAction]
        public bool Exists(string cacheKey)
        {
            return _cache.Equals(cacheKey);
        }

        /// <summary>
        /// 增加缓存Key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [NonAction]
        public async Task AddCacheKey(string cacheKey)
        {
            var res = await _cache.GetStringAsync(CommonConst.CACHE_KEY_ALL);
            var allkeys = string.IsNullOrWhiteSpace(res) ? new List<string>() : JSON.Deserialize<List<string>>(res);
            allkeys.Add(cacheKey);
            await _cache.SetStringAsync(CommonConst.CACHE_KEY_ALL, JSON.Serialize(allkeys));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [NonAction]
        public async Task DelCacheKey(string cacheKey)
        {
            var res = await _cache.GetStringAsync(CommonConst.CACHE_KEY_ALL);
            var allkeys = string.IsNullOrWhiteSpace(res) ? new List<string>() : JSON.Deserialize<List<string>>(res);
            allkeys.Remove(cacheKey);
            await _cache.SetStringAsync(CommonConst.CACHE_KEY_ALL, JSON.Serialize(allkeys));
        }
    }
}