using Microsoft.Extensions.Caching.Memory;

namespace Admin.NET.Core;

/// <summary>
/// SqlSugar二级缓存
/// </summary>
public class SqlSugarCache : ICacheService, ISingleton, IDisposable
{
    private static readonly Lazy<IMemoryCache> lazyCache = new(() => new MemoryCache(new MemoryCacheOptions()));
    public static IMemoryCache _cache => lazyCache.Value;

    public void Add<V>(string key, V value)
    {
        _cache.Set(key, value);
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        _cache.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
    }

    public bool ContainsKey<V>(string key)
    {
        return _cache.TryGetValue(key, out _);
    }

    public V Get<V>(string key)
    {
        return _cache.Get<V>(key);
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
        var cacheItems = entries as IDictionary;
        var keys = new List<string>();
        if (cacheItems == null) return keys;
        foreach (DictionaryEntry cacheItem in cacheItems)
        {
            keys.Add(cacheItem.Key.ToString());
        }
        return keys;
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (!_cache.TryGetValue<V>(cacheKey, out V value))
        {
            value = create();
            _cache.Set(cacheKey, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
        }
        return value;
    }

    public void Remove<V>(string key)
    {
        _cache.Remove(key);
    }

    public void Dispose()
    {
        _cache.Dispose();
    }
}