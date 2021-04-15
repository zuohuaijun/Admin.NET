using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace Dilon.Core
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class MemoryCache : ICache, ISingleton
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public long Del(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }
            return key.Length;
        }

        public Task<long> DelAsync(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }

            return Task.FromResult((long)key.Length);
        }

        public async Task<long> DelByPatternAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return default;

            //pattern = Regex.Replace(pattern, @"\{*.\}", "(.*)");
            var keys = GetAllKeys().Where(k => k.StartsWith(pattern));
            if (keys != null && keys.Any())
                return await DelAsync(keys.ToArray());

            return default;
        }

        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public string Get(string key)
        {
            return _memoryCache.Get(key)?.ToString();
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(Get(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public bool Set(string key, object value)
        {
            _memoryCache.Set(key, value);
            return true;
        }

        public bool Set(string key, object value, TimeSpan expire)
        {
            _memoryCache.Set(key, value, expire);
            return true;
        }

        public Task<bool> SetAsync(string key, object value)
        {
            Set(key, value);
            return Task.FromResult(true);
        }

        public Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
            Set(key, value, expire);
            return Task.FromResult(true);
        }

        public List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries.GetType().GetProperty("Keys").GetValue(entries) as ICollection<object>; //entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            return cacheItems.Select(u => u.ToString()).ToList();
            //foreach (DictionaryEntry cacheItem in cacheItems)
            //{
            //    keys.Add(cacheItem.Key.ToString());
            //}
            //return keys;
        }
    }
}