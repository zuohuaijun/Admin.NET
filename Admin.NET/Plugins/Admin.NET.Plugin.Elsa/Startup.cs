// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Elsa;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.NET.Plugin.Elsa;

[AppStartup(100)]
public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //var elsaOptions = App.GetOptions<ElsaOptions>();
        services
            .AddElsa(options => options
                .AddActivitiesFrom<Startup>()
                .AddWorkflowsFrom<Startup>()
                // .AddFeatures(startups, Configuration)
                // .ConfigureWorkflowChannels(options => elsaSection.GetSection("WorkflowChannels").Bind(options))
                .AddHttpActivities(App.Configuration.GetSection("Elsa").GetSection("Server").Bind)
            );

        services
            .AddNotificationHandlersFrom<Startup>()
            .AddElsaApiEndpoints()
            .AddElsaSwagger(options =>
            {
                //options.SwaggerDoc("Elsa", new OpenApiInfo() { Title = "Elsa", Description = "https://v2.elsaworkflows.io/" });
                //options.TagActionsBy(api => new[] { new OpenApiTag { Name = "Elsa", Description = "Elsa Core API Endpoints" } });
                options.TagActionsBy(api => new[] { "Elsa" });
                options.DocInclusionPredicate((docName, description) => true);
            });

        services.AddApiVersioning(options =>
        {
            options.UseApiBehavior = false;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseApiVersioning();
        app.UseHttpActivities();
        app.UseElsaFeatures();
    }
}