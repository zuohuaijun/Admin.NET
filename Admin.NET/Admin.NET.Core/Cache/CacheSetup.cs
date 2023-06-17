namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static ICache AddCache(this IServiceCollection services)
    {
        ICache cache = Cache.Default;

        var cacheOptions = App.GetOptions<CacheOptions>();
        if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
        {
            var redis = new FullRedis();
            redis.Init(cacheOptions.RedisConnectionString);
            cache = redis;
        }

        services.AddSingleton(cache);
        return cache;
    }
}