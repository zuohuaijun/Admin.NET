using CSRedis;
using Microsoft.Extensions.Caching.Redis;

namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（Redis缓存）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        var cacheOptions = App.GetOptions<CacheOptions>();
        if (cacheOptions.CacheType != CacheTypeEnum.RedisCache.ToString())
            return;

        services.AddSingleton<IDistributedCache>(provider =>
        {
            var redisStr = $"{cacheOptions.RedisConnectionString},prefix={cacheOptions.InstanceName}";
            var redis = new CSRedisClient(redisStr);
            RedisHelper.Initialization(redis);
            return new CSRedisCache(redis);
        });
    }
}