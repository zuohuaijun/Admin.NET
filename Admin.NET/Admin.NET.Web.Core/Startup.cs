using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion;
using Furion.FriendlyException;
using Furion.Logging.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnceMi.AspNetCore.OSS;
using Yitter.IdGenerator;

namespace Admin.NET.Web.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 注册配置选项
        services.AddProjectOptions();
        // 注册ORM
        services.AddSqlSugarSetup(App.Configuration);
        // 注册JWT
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
        // 注册跨域
        services.AddCorsAccessor();
        // 注册远程请求
        services.AddRemoteRequest();
        // 注册任务调度
        services.AddTaskScheduler();
        // 注册脱敏检测
        services.AddSensitiveDetection();

        services.AddControllersWithViews()
            .AddMvcFilter<ActionFilter>()
            .AddMvcFilter<ResultFilter>()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 响应驼峰命名
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
            })
            .AddInjectWithUnifyResult<AdminResultProvider>();

        // 注册事件总线
        services.AddEventBus(builder =>
        {
            builder.AddSubscriber<LogEventSubscriber>();
        });
        // 注册OSS对象存储
        services.AddOSSService(options =>
        {
            options = App.GetOptions<OSSProviderOptions>();
        });
        // 注册邮件
        services.AddMailKit(options =>
        {
            options.UseMailKit(App.GetOptions<EmailOptions>());
        });

        // 注册Redis缓存
        services.AddCSRedisSetup();

        // 注册模板引擎
        services.AddViewEngine();

        // 注册即时通讯
        services.AddSignalR();

        // 注册logo显示
        services.AddLogoDisplay();

        // 注册日志
        services.AddFileLogging();
        services.AddFileLogging("logs/error.log", options =>
        {
            options.WriteFilter = (logMsg) =>
            {
                return logMsg.LogLevel == LogLevel.Error;
            };
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // 添加状态码拦截中间件
        app.UseUnifyResultStatusCodes();

        // 启用HTTPS
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // HTTP请求日志
        app.UseHttpLogging();

        app.UseRouting();

        app.UseCorsAccessor();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseInject();

        app.UseEndpoints(endpoints =>
        {
            // 注册集线器
            endpoints.MapHubs();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        // 设置雪花Id算法机器码
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            WorkerId = App.GetOptions<SnowIdOptions>().WorkerId
        });
    }
}