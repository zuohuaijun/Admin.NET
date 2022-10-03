using Admin.NET.Core;
using AspNetCoreRateLimit;
using Furion;
using Furion.SpecificationDocument;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnceMi.AspNetCore.OSS;
using System;
using System.IO;
using Yitter.IdGenerator;

namespace Admin.NET.Web.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 配置选项
        services.AddProjectOptions();
        // 缓存注册
        services.AddCache();
        // SqlSugar
        services.AddSqlSugar();
        // JWT
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
        // 允许跨域
        services.AddCorsAccessor();
        // 远程请求
        services.AddRemoteRequest();
        // 任务调度
        services.AddTaskScheduler();
        // 脱敏检测
        services.AddSensitiveDetection();
        //// 结果拦截器
        //services.AddMvcFilter<ResultFilter>();
        // 日志监听
        services.AddMonitorLogging();

        services.AddControllersWithViews()
            .AddAppLocalization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 首字母小写（驼峰样式）
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                // options.SerializerSettings.Converters.Add(new LongJsonConverter()); // long转string（防止js精度溢出） 超过16位开启
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
            })
            .AddInjectWithUnifyResult<AdminResultProvider>();

        // 第三方授权登录
        services.AddAuthentication()
            .AddWeixin(options =>
            {
                var opt = App.GetOptions<OAuthOptions>();
                options.ClientId = opt.Weixin.ClientId;
                options.ClientSecret = opt.Weixin.ClientSecret;
            });

        // ElasticSearch
        services.AddElasticSearch();

        // 配置Nginx转发获取客户端真实IP
        // 注1：如果负载均衡不是在本机通过 Loopback 地址转发请求的，一定要加上options.KnownNetworks.Clear()和options.KnownProxies.Clear()
        // 注2：如果设置环境变量 ASPNETCORE_FORWARDEDHEADERS_ENABLED 为 True，则不需要下面的配置代码
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        // 限流服务
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        // 事件总线
        services.AddEventBus(options =>
        {
            // 不启用事件日志
            options.LogEnabled = false;
            // 事件执行器（失败重试）
            options.AddExecutor<RetryEventHandlerExecutor>();
            //// 替换事件源存储器
            //options.ReplaceStorer(serviceProvider =>
            //{
            //    var redisClient = serviceProvider.GetService<ICache>();
            //    return new RedisEventSourceStorer(redisClient);
            //});
        });

        // OSS对象存储（必须一个个赋值）
        var ossOpt = App.GetOptions<OSSProviderOptions>();
        services.AddOSSService(Enum.GetName(ossOpt.Provider), options =>
        {
            options.Provider = ossOpt.Provider;
            options.Endpoint = ossOpt.Endpoint;
            options.AccessKey = ossOpt.AccessKey;
            options.SecretKey = ossOpt.SecretKey;
            options.Region = ossOpt.Region;
            options.IsEnableCache = ossOpt.IsEnableCache;
            options.IsEnableHttps = ossOpt.IsEnableHttps;
        });

        // 电子邮件
        services.AddMailKit(options =>
        {
            options.UseMailKit(App.GetOptions<EmailOptions>());
        });

        // 模板引擎
        services.AddViewEngine();

        // 即时通讯
        services.AddSignalR();

        // logo显示
        services.AddLogoDisplay();

        // 日志记录
        if (App.GetConfig<bool>("Logging:File:Enabled")) // 日志写入文件
        {
            Array.ForEach(new[] { LogLevel.Information, LogLevel.Warning, LogLevel.Error }, logLevel =>
            {
                services.AddFileLogging(options =>
                {
                    options.FileNameRule = fileName => string.Format(fileName, DateTime.Now, logLevel.ToString()); // 每天创建一个文件
                    options.WriteFilter = logMsg => logMsg.LogLevel == logLevel; // 日志级别
                    options.HandleWriteError = (writeError) => // 写入失败时启用备用文件
                    {
                        writeError.UseRollbackFileName(Path.GetFileNameWithoutExtension(writeError.CurrentFileName) + "-oops" + Path.GetExtension(writeError.CurrentFileName));
                    };
                });
            });
        }
        if (App.GetConfig<bool>("Logging:Database:Enabled")) // 日志写入数据库
        {
            services.AddDatabaseLogging<DatabaseLoggingWriter>();
        }
        if (App.GetConfig<bool>("Logging:ElasticSearch:Enabled")) // 日志写入ElasticSearch
        {
            services.AddDatabaseLogging<ElasticSearchLoggingWriter>(options =>
            {
                options.MessageFormat = LoggerFormatter.Json;
            });
        }

        // 配置雪花Id算法机器码
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            WorkerId = App.GetOptions<SnowIdOptions>().WorkerId
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseForwardedHeaders();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseForwardedHeaders();
            app.UseHsts();
        }

        // 添加状态码拦截中间件
        app.UseUnifyResultStatusCodes();

        // 配置多语言
        app.UseAppLocalization();

        // 启用HTTPS
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCorsAccessor();

        // 限流组件（在跨域之后）
        app.UseIpRateLimiting();
        app.UseClientRateLimiting();

        app.UseAuthentication();
        app.UseAuthorization();

        // 配置Swagger-Knife4UI（路由前缀一致代表独立，不同则代表共存）
        app.UseKnife4UI(options =>
        {
            options.RoutePrefix = "kapi";
            foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
            {
                options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
            }
        });

        app.UseInject();

        app.UseEndpoints(endpoints =>
        {
            // 注册集线器
            endpoints.MapHubs();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}