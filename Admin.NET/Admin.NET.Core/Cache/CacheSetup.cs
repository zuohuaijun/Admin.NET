// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        ICache cache = Cache.Default;

        var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
        {
            cache = new FullRedis(new RedisOptions
            {
                Configuration = cacheOptions.Redis.Configuration,
                Prefix = cacheOptions.Redis.Prefix
            });
            if (cacheOptions.Redis.MaxMessageSize > 0)
                ((FullRedis)cache).MaxMessageSize = cacheOptions.Redis.MaxMessageSize;
        }

        services.AddSingleton(cache);
    }
}