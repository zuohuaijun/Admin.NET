using Admin.NET.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.NET.Web.Core;

public static class ProjectOptions
{
    /// <summary>
    /// 注册项目配置选项
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddProjectOptions(this IServiceCollection services)
    {
        services.AddConfigurableOptions<DbConnectionOptions>();
        services.AddConfigurableOptions<RefreshTokenOptions>();
        services.AddConfigurableOptions<SnowIdOptions>();
        services.AddConfigurableOptions<CacheOptions>();
        services.AddConfigurableOptions<OSSProviderOptions>();
        services.AddConfigurableOptions<UploadOptions>();
        services.AddConfigurableOptions<WechatOptions>();
        services.AddConfigurableOptions<WechatPayOptions>();
        services.AddConfigurableOptions<PayCallBackOptions>();
        services.AddConfigurableOptions<CodeGenOptions>();

        return services;
    }
}