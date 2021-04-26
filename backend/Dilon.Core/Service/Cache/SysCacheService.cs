using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 系统缓存服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Cache", Order = 100)]
    public class SysCacheService : ISysCacheService, IDynamicApiController, ISingleton
    {
        private readonly ICache _cache;
        private readonly CacheOptions _cacheOptions;

        public SysCacheService(IOptions<CacheOptions> cacheOptions, Func<string, ISingleton, object> resolveNamed)
        {
            _cacheOptions = cacheOptions.Value;
            _cache = resolveNamed(_cacheOptions.CacheType.ToString(), default) as ICache;
        }

        /// <summary>
        /// 获取数据范围缓存（机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetDataScope(long userId)
        {
            var cacheKey = CommonConst.CACHE_KEY_DATASCOPE + $"{userId}";
            return await _cache.GetAsync<List<long>>(cacheKey);
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
            await _cache.SetAsync(cacheKey, dataScopes);
        }

        /// <summary>
        /// 获取菜单缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public async Task<List<AntDesignTreeNode>> GetMenu(long userId, string appCode)
        {
            var cacheKey = CommonConst.CACHE_KEY_MENU + $"{userId}-{appCode}";
            return await _cache.GetAsync<List<AntDesignTreeNode>>(cacheKey);
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
            await _cache.SetAsync(cacheKey, menus);
        }

        /// <summary>
        /// 获取权限缓存（按钮）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetPermission(long userId)
        {
            var cacheKey = CommonConst.CACHE_KEY_PERMISSION + $"{userId}";
            return await _cache.GetAsync<List<string>>(cacheKey);
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
            await _cache.SetAsync(cacheKey, permissions);
        }

        /// <summary>
        /// 获取所有缓存关键字
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCacheKeys()
        {
            var cacheItems = _cache.GetAllKeys();
            if (cacheItems == null) return new List<string>();
            return cacheItems.Where(u => !u.ToString().StartsWith("mini-profiler")).Select(u => u).ToList();
        }

        /// <summary>
        /// 删除指定关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public bool Del(string key)
        {
            _cache.Del(key);
            return true;
        }

        /// <summary>
        /// 删除指定关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> DelAsync(string key)
        {
            _cache.DelAsync(key);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 删除某特征关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> DelByPatternAsync(string key)
        {
            _cache.DelByPatternAsync(key);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        public bool Set(string key, object value)
        {
            return _cache.Set(key, value);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, object value)
        {
            return await _cache.SetAsync(key, value);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public string Get(string key)
        {
            return _cache.Get(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            return await _cache.GetAsync(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string key)
        {
            return _cache.GetAsync<T>(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        [NonAction]
        public bool Exists(string key)
        {
            return _cache.Exists(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string key)
        {
            return _cache.ExistsAsync(key);
        }
    }
}