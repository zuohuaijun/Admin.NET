namespace Admin.NET.Core;

/// <summary>
/// SqlSugar二级缓存
/// </summary>
public class SqlSugarCache : ICacheService
{
    private static IDistributedCache _cache = App.GetService<IDistributedCache>();

    public void Add<V>(string key, V value)
    {
        _cache.Set(key, Encoding.UTF8.GetBytes(JSON.Serialize(value)));
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        _cache.Set(key, Encoding.UTF8.GetBytes(JSON.Serialize(value)), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds) });
    }

    public bool ContainsKey<V>(string key)
    {
        return _cache.Get(key) != null;
    }

    public V Get<V>(string key)
    {
        var res = _cache.Get(key);
        return res == null ? default : JSON.Deserialize<V>(Encoding.UTF8.GetString(res));
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var entries = _cache.GetType().GetField("_entries", flags)?.GetValue(_cache);
        var cacheItems = entries?.GetType().GetProperty("Keys").GetValue(entries) as ICollection<object>;
        return cacheItems == null ? new List<string>() : cacheItems.Select(u => u.ToString()).ToList();
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (!ContainsKey<V>(cacheKey))
            return JSON.Deserialize<V>(Encoding.UTF8.GetString(_cache.Get(cacheKey)));

        var result = create();
        Add(cacheKey, result, cacheDurationInSeconds);
        return result;
    }

    public void Remove<V>(string key)
    {
        _cache.Remove(key);
    }
}