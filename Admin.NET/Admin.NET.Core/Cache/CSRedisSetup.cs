using CSRedis;
using Furion;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Admin.NET.Core
{
    public static class CSRedisSetup
    {
        /// <summary>
        /// CSRedis初始化
        /// </summary>
        /// <param name="services"></param>
        public static void AddCSRedisSetup(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache>(provider =>
            {
                var cacheOptions = App.GetOptions<CacheOptions>();
                if (cacheOptions.CacheType == CacheTypeEnum.RedisCache.ToString())
                {
                    var redisStr = $"{cacheOptions.RedisConnectionString},prefix={cacheOptions.InstanceName}";

                    var redis = new CSRedisClient(redisStr);
                    services.AddSingleton(redis);
                    RedisHelper.Initialization(redis);

                    return new CSRedisCache(redis);
                }
                else // 默认使用内存
                {
                    services.AddDistributedMemoryCache();
                    IOptions<MemoryDistributedCacheOptions> options = provider.GetService<IOptions<MemoryDistributedCacheOptions>>();
                    return new MemoryDistributedCache(options);
                }
            });
        }
    }
}