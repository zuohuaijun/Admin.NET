// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
            u.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
            u.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
            u.Limits.MaxRequestBodySize = null;
        });
    }
}