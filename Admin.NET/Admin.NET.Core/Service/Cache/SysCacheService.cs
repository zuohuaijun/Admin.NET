namespace Admin.NET.Core.Service;

/// <summary>
/// 系统缓存服务
/// </summary>
[ApiDescriptionSettings(Order = 400)]
public class SysCacheService : IDynamicApiController, ISingleton
{
    private readonly ICache _cache;

    public SysCacheService(ICache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 获取缓存键名集合
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "KeyList")]
    [DisplayName("获取缓存键名集合")]
    public List<string> GetKeyList()
    {
        return _cache.Keys.ToList();
    }

    /// <summary>
    /// 增加缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public void Set(string key, object value)
    {
        _cache.Set(key, value);
    }

    /// <summary>
    /// 增加缓存并设置过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public void Set(string key, object value, TimeSpan expire)
    {
        _cache.Set(key, value, expire);
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete")]
    [DisplayName("删除缓存")]
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// 检查缓存是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public bool ExistKey(string key)
    {
        return _cache.ContainsKey(key);
    }

    /// <summary>
    /// 根据键名前缀删除缓存
    /// </summary>
    /// <param name="prefixKey">键名前缀</param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DeleteByPreKey")]
    [DisplayName("根据键名前缀删除缓存")]
    public int RemoveByPrefixKey(string prefixKey)
    {
        var delKeys = _cache.Keys.Where(u => u.StartsWith(prefixKey)).ToArray();
        if (!delKeys.Any()) return 0;
        return _cache.Remove(delKeys);
    }

    /// <summary>
    /// 获取缓存值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Value")]
    [DisplayName("获取缓存值")]
    public dynamic GetValue(string key)
    {
        return _cache.Get<dynamic>(key);
    }
}