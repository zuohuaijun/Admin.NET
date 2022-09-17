using NewLife.Caching;

namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        services.AddSingleton<ICache>(options =>
        {
            var cacheOptions = App.GetOptions<CacheOptions>();
            if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
            {
                var redis = new Redis();
                redis.Init(cacheOptions.RedisConnectionString);
                return redis;
            }
            else
            {
                return Cache.Default;
            }
        });
    }
}