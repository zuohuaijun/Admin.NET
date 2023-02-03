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
    /// 获取所有缓存键名
    /// </summary>
    /// <returns></returns>
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
    [ApiDescriptionSettings(Name = "Add")]
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
    [ApiDescriptionSettings(Name = "AddAndSetExp")]
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
    [NonAction]
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
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// 检查缓存是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    [NonAction]
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
    public dynamic GetValue(string key)
    {
        return _cache.Get<dynamic>(key);
    }
}