// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统缓存服务
/// </summary>
[ApiDescriptionSettings(Order = 400)]
public class SysCacheService : IDynamicApiController, ISingleton
{
    private readonly ICache _cache;
    private readonly CacheOptions _cacheOptions;

    public SysCacheService(ICache cache, IOptions<CacheOptions> cacheOptions)
    {
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
    }

    /// <summary>
    /// 获取缓存键名集合
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取缓存键名集合")]
    public List<string> GetKeyList()
    {
        return _cache == Cache.Default
            ? _cache.Keys.Where(u => u.StartsWith(_cacheOptions.Prefix)).Select(u => u[_cacheOptions.Prefix.Length..]).OrderBy(u => u).ToList()
            : ((FullRedis)_cache).Search($"{_cacheOptions.Prefix}*", int.MaxValue).Select(u => u[_cacheOptions.Prefix.Length..]).OrderBy(u => u).ToList();
    }

    /// <summary>
    /// 增加缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [NonAction]
    public bool Set(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key)) return false;

        return _cache.Set($"{_cacheOptions.Prefix}{key}", value);
    }

    /// <summary>
    /// 增加缓存并设置过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    [NonAction]
    public bool Set(string key, object value, TimeSpan expire)
    {
        if (string.IsNullOrWhiteSpace(key)) return false;

        return _cache.Set($"{_cacheOptions.Prefix}{key}", value, expire);
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
        return _cache.Get<T>($"{_cacheOptions.Prefix}{key}");
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除缓存")]
    public int Remove(string key)
    {
        return _cache.Remove($"{_cacheOptions.Prefix}{key}");
    }

    /// <summary>
    /// 检查缓存是否存在
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    [NonAction]
    public bool ExistKey(string key)
    {
        return _cache.ContainsKey($"{_cacheOptions.Prefix}{key}");
    }

    /// <summary>
    /// 根据键名前缀删除缓存
    /// </summary>
    /// <param name="prefixKey">键名前缀</param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DeleteByPreKey"), HttpPost]
    [DisplayName("根据键名前缀删除缓存")]
    public int RemoveByPrefixKey(string prefixKey)
    {
        var delKeys = _cache == Cache.Default
            ? _cache.Keys.Where(u => u.StartsWith($"{_cacheOptions.Prefix}{prefixKey}")).ToArray()
            : ((FullRedis)_cache).Search($"{_cacheOptions.Prefix}{prefixKey}*", int.MaxValue).ToArray();

        return _cache.Remove(delKeys);
    }

    /// <summary>
    /// 根据键名前缀获取键名集合
    /// </summary>
    /// <param name="prefixKey">键名前缀</param>
    /// <returns></returns>
    [DisplayName("根据键名前缀获取键名集合")]
    public List<string> GetKeysByPrefixKey(string prefixKey)
    {
        return _cache == Cache.Default
            ? _cache.Keys.Where(u => u.StartsWith($"{_cacheOptions.Prefix}{prefixKey}")).Select(u => u[_cacheOptions.Prefix.Length..]).ToList()
            : ((FullRedis)_cache).Search($"{_cacheOptions.Prefix}{prefixKey}*", int.MaxValue).Select(u => u[_cacheOptions.Prefix.Length..]).ToList();
    }

    /// <summary>
    /// 获取缓存值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [DisplayName("获取缓存值")]
    public object GetValue(string key)
    {
        return _cache == Cache.Default
            ? _cache.Get<object>($"{_cacheOptions.Prefix}{key}")
            : _cache.Get<string>($"{_cacheOptions.Prefix}{key}");
    }

    /// <summary>
    /// 获取或添加缓存，在数据不存在时执行委托请求数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    /// <param name="expire">过期时间，单位秒</param>
    /// <returns></returns>
    [NonAction]
    public T GetOrAdd<T>(string key, Func<string, T> callback, int expire = -1)
    {
        if (string.IsNullOrWhiteSpace(key)) return default;

        return _cache.GetOrAdd($"{_cacheOptions.Prefix}{key}", callback, expire);
    }
}