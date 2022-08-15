namespace Admin.NET.Core.Service;

/// <summary>
/// 系统缓存服务
/// </summary>
[ApiDescriptionSettings(Name = "系统缓存", Order = 190)]
public class SysCacheService : ISysCacheService, IDynamicApiController, ISingleton
{
    private readonly IDistributedCache _cache;

    public SysCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 获取所有缓存列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysCache/keyList")]
    public async Task<List<string>> GetAllCacheKeys()
    {
        var res = await _cache.GetStringAsync(CacheConst.KeyAll);
        return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<string>>(res);
    }

    /// <summary>
    /// 增加对象缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("/sysCache/addObject")]
    public async Task SetAsync(string cacheKey, object value)
    {
        await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JSON.Serialize(value)));

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    /// 增加对象缓存,并设置过期时间
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    [HttpPost("/sysCache/addObject/expire")]
    public async Task SetAsync(string cacheKey, object value, TimeSpan expire)
    {
        await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JSON.Serialize(value)), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    /// 增加字符串缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPost("/sysCache/addString")]
    public async Task SetStringAsync(string cacheKey, string value)
    {
        await _cache.SetStringAsync(cacheKey, value);

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    /// 增加字符串缓存,并设置过期时间
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="value"></param>
    /// <param name="expire"></param>
    /// <returns></returns>
    [HttpPost("/sysCache/addString/expire")]
    public async Task SetStringAsync(string cacheKey, string value, TimeSpan expire)
    {
        await _cache.SetStringAsync(cacheKey, value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    [HttpGet("/sysCache/detail")]
    public async Task<string> GetStringAsync(string cacheKey)
    {
        return await _cache.GetStringAsync(cacheKey);
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("/sysCache/remove")]
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
        if (allkeys == null) return;

        var delAllkeys = allkeys.Where(u => u.Contains(key)).ToList();
        delAllkeys.ForEach(u =>
        {
            _cache.Remove(u);
        });

        // 更新所有缓存键
        allkeys = allkeys.Where(u => !u.Contains(key)).ToList();
        await _cache.SetStringAsync(CacheConst.KeyAll, JSON.Serialize(allkeys));
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
    public async Task<bool> ExistsAsync(string cacheKey)
    {
        var res = await _cache.GetAsync(cacheKey);
        return res != null;
    }

    /// <summary>
    /// 增加缓存Key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    [NonAction]
    public async Task AddCacheKey(string cacheKey)
    {
        var res = await _cache.GetStringAsync(CacheConst.KeyAll);
        var allkeys = string.IsNullOrWhiteSpace(res) ? new List<string>() : JSON.Deserialize<List<string>>(res);
        if (!allkeys.Any(m => m == cacheKey))
        {
            allkeys.Add(cacheKey);
            await _cache.SetStringAsync(CacheConst.KeyAll, JSON.Serialize(allkeys));
        }
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    [NonAction]
    public async Task DelCacheKey(string cacheKey)
    {
        var res = await _cache.GetStringAsync(CacheConst.KeyAll);
        var allkeys = string.IsNullOrWhiteSpace(res) ? new List<string>() : JSON.Deserialize<List<string>>(res);
        if (allkeys.Any(m => m == cacheKey))
        {
            allkeys.Remove(cacheKey);
            await _cache.SetStringAsync(CacheConst.KeyAll, JSON.Serialize(allkeys));
        }
    }

    /// <summary>
    /// 获取机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetOrgIdList(long userId)
    {
        var cacheKey = CacheConst.KeyOrgIdList + userId;
        var res = await _cache.GetStringAsync(cacheKey);
        return string.IsNullOrWhiteSpace(res) ? null : JSON.Deserialize<List<long>>(res);
    }

    /// <summary>
    /// 缓存机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    [NonAction]
    public async Task SetOrgIdList(long userId, List<long> orgIdList)
    {
        var cacheKey = CacheConst.KeyOrgIdList + userId;
        await _cache.SetStringAsync(cacheKey, JSON.Serialize(orgIdList));

        await AddCacheKey(cacheKey);
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

    //    await AddCacheKey(cacheKey);
    //}

    /// <summary>
    /// 获取权限缓存（按钮）
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<string>> GetPermission(long userId)
    {
        var cacheKey = CacheConst.KeyPermission + userId;
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
        var cacheKey = CacheConst.KeyPermission + userId;
        await _cache.SetStringAsync(cacheKey, JSON.Serialize(permissions));

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    /// 获取最大角色数据范围
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<int?> GetMaxDataScopeType(long userId)
    {
        var cacheKey = CacheConst.KeyMaxDataScopeType + userId;
        var res = await _cache.GetStringAsync(cacheKey);
        return string.IsNullOrWhiteSpace(res) ? null : int.Parse(res);
    }

    /// <summary>
    /// 缓存最大角色数据范围
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dataScopeType"></param>
    /// <returns></returns>
    [NonAction]
    public async Task SetMaxDataScopeType(long userId, int dataScopeType)
    {
        var cacheKey = CacheConst.KeyMaxDataScopeType + userId;
        await _cache.SetStringAsync(cacheKey, dataScopeType.ToString());

        await AddCacheKey(cacheKey);
    }

    /// <summary>
    ///  根据父键清空
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("/sysCache/delByParentKey")]
    public async Task DelByParentKeyAsync(string key)
    {
        var allkeys = await GetAllCacheKeys();
        if (allkeys == null) return;

        var delAllkeys = allkeys.Where(u => u.StartsWith(key)).ToList();
        delAllkeys.ForEach(u =>
        {
            _cache.Remove(u);
        });
        // 更新所有缓存键
        allkeys = allkeys.Where(u => !u.StartsWith(key)).ToList();
        await _cache.SetStringAsync(CacheConst.KeyAll, JSON.Serialize(allkeys));
    }
}