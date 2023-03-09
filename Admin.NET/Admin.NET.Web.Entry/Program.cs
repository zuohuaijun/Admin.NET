using Microsoft.Extensions.Options;

Serve.Run(RunOptions.Default.AddWebComponent<WebComponent>());

public class WebComponent : IWebComponent
{
    public void Load(WebApplicationBuilder builder, ComponentContext componentContext)
    {
        // 设置日志过滤
        builder.Logging.AddFilter((provider, category, logLevel) =>
        {
            return !new[] { "Microsoft.Hosting", "Microsoft.AspNetCore" }.Any(u => category.StartsWith(u)) && logLevel >= LogLevel.Information;
        });

        // 设置接口超时时间和上传大小
        builder.Configuration.Get<WebHostBuilder>().ConfigureKestrel(u =>
        {
            //u.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
            //u.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
            u.Limits.MaxRequestBodySize = null;
        });
    }
}