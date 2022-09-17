using NewLife.Caching;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统缓存服务
/// </summary>
[ApiDescriptionSettings(Order = 190)]
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
    [HttpGet("/sysCache/keyList")]
    public List<string> GetCacheKeys()
    {
        return _cache.Keys.ToList();
    }

    /// <summary>
    /// 增加缓存
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("/sysCache/add")]
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
    [HttpPost("/sysCache/add/expire")]
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
    [HttpGet("/sysCache/remove")]
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
    [HttpGet("/sysCache/delByParentKey")]
    public int RemoveByPrefixKey(string prefixKey)
    {
        var delKeys = _cache.Keys.Where(u => u.StartsWith(prefixKey)).ToArray();
        return _cache.Remove(delKeys);
    }

    /// <summary>
    /// 获取机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public List<long> GetOrgIdList(long userId)
    {
        var key = CacheConst.KeyOrgIdList + userId;
        return _cache.Get<List<long>>(key);
    }

    /// <summary>
    /// 缓存机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    [NonAction]
    public bool SetOrgIdList(long userId, List<long> orgIdList)
    {
        var key = CacheConst.KeyOrgIdList + userId;
        return _cache.Set(key, orgIdList);
    }

    /// <summary>
    /// 获取权限集合（按钮）
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public List<string> GetPermission(long userId)
    {
        var key = CacheConst.KeyPermission + userId;
        return _cache.Get<List<string>>(key);
    }

    /// <summary>
    /// 缓存权限集合（按钮）
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="permissions"></param>
    /// <returns></returns>
    [NonAction]
    public bool SetPermission(long userId, List<string> permissions)
    {
        var key = CacheConst.KeyPermission + userId;
        return _cache.Set(key, permissions);
    }

    /// <summary>
    /// 获取最大角色数据范围
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public int? GetMaxDataScopeType(long userId)
    {
        var key = CacheConst.KeyMaxDataScopeType + userId;
        return _cache.Get<int>(key);
    }

    /// <summary>
    /// 缓存最大角色数据范围
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dataScopeType"></param>
    /// <returns></returns>
    [NonAction]
    public bool SetMaxDataScopeType(long userId, int dataScopeType)
    {
        var key = CacheConst.KeyMaxDataScopeType + userId;
        return _cache.Set(key, dataScopeType);
    }

    ///// <summary>
    ///// 获取菜单缓存
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <param name="appCode"></param>
    ///// <returns></returns>
    //[NonAction]
    //public async Task<List<AntDesignTreeNode>> GetMenu(long userId, string appCode)
    //{
    //    var cacheKey = CacheConst.KeyMenu + $"{userId}-{appCode}";
    //    var res = await _cache.GetStringAsync(cacheKey);
    //    return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<AntDesignTreeNode>>(res);
    //}

    ///// <summary>
    ///// 缓存菜单
    ///// </summary>
    ///// <param name="userId"></param>
    ///// <param name="appCode"></param>
    ///// <param name="menus"></param>
    ///// <returns></returns>
    //[NonAction]
    //public async Task SetMenu(long userId, string appCode, List<AntDesignTreeNode> menus)
    //{
    //    var cacheKey = CommonConst.CACHE_KEY_MENU + $"{userId}-{appCode}";
    //    await _cache.SetStringAsync(cacheKey, JSON.Serialize(menus));
    //}
}