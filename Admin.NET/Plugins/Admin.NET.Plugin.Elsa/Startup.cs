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