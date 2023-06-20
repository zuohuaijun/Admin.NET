using Admin.NET.Core;
using Admin.NET.Core.Service;
using AspNetCoreRateLimit;
using Furion;
using Furion.SpecificationDocument;
using Furion.VirtualFileServer;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnceMi.AspNetCore.OSS;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
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
        // 任务队列
        services.AddTaskQueue();
        // 任务调度
        services.AddSchedule(options =>
        {
            options.AddPersistence<DbJobPersistence>(); // 添加作业持久化器
        });
        // 脱敏检测
        services.AddSensitiveDetection();
        // 控制台格式化
        services.AddConsoleFormatter(options =>
        {
            options.DateFormat = "yyyy-MM-dd HH:mm:ss(zzz) dddd";
        });
        // 日志监听
        services.AddMonitorLogging(options =>
        {
            options.IgnorePropertyNames = new[] { "Byte" };
            options.IgnorePropertyTypes = new[] { typeof(byte[]) };
        });

        services.AddControllersWithViews()
            .AddAppLocalization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 解决动态对象属性名大写
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
                // options.SerializerSettings.Converters.AddLongTypeConverters(); // long转string（防止js精度溢出） 超过16位开启
                // options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore; // 解决DateTimeOffset异常
                // options.SerializerSettings.DateParseHandling = DateParseHandling.None; // 解决DateTimeOffset异常
                // options.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }); // 解决DateTimeOffset异常
            })
            //.AddXmlSerializerFormatters()
            //.AddXmlDataContractSerializerFormatters()
            .AddInjectWithUnifyResult<AdminResultProvider>();

        //// 第三方授权登录
        //services.AddAuthentication()
        //    .AddWeixin(options =>
        //    {
        //        var opt = App.GetOptions<OAuthOptions>();
        //        options.ClientId = opt.Weixin.ClientId;
        //        options.ClientSecret = opt.Weixin.ClientSecret;
        //    });

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
            options.UseUtcTimestamp = false;
            // 不启用事件日志
            options.LogEnabled = false;
            // 事件执行器（失败重试）
            options.AddExecutor<RetryEventHandlerExecutor>();
            //// 替换事件源存储器
            //options.ReplaceStorer(serviceProvider =>
            //{
            //    var redisCache = serviceProvider.GetService<ICache>();
            //    // 创建默认内存通道事件源对象，可自定义队列路由key，比如这里是 eventbus
            //    return new RedisEventSourceStorer(redisCache, "eventbus", 3000);
            //});
            options.AddSubscriber<AppEventSubscriber>();
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
        var emailOpt = App.GetOptions<EmailOptions>();
        services.AddFluentEmail(emailOpt.DefaultFromEmail, emailOpt.DefaultFromName)
            .AddSmtpSender(new SmtpClient(emailOpt.Host, emailOpt.Port)
            {
                EnableSsl = emailOpt.EnableSsl,
                UseDefaultCredentials = emailOpt.UseDefaultCredentials,
                Credentials = new NetworkCredential(emailOpt.UserName, emailOpt.Password)
            });

        // 模板引擎
        services.AddViewEngine();

        // 即时通讯
        services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            })
            .AddJsonProtocol();

        // logo显示
        services.AddLogoDisplay();

        // 日志记录
        if (App.GetConfig<bool>("Logging:File:Enabled")) // 日志写入文件
        {
            Array.ForEach(new[] { LogLevel.Information, LogLevel.Warning, LogLevel.Error }, logLevel =>
            {
                services.AddFileLogging(options =>
                {
                    options.WithStackFrame = true; // 显示堆栈信息
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
            services.AddDatabaseLogging<DatabaseLoggingWriter>(options =>
            {
                options.WithStackFrame = true; // 显示堆栈信息
                options.WithTraceId = true; // 显示线程Id
                options.IgnoreReferenceLoop = false; // 忽略循环检测
                options.WriteFilter = (logMsg) =>
                {
                    return logMsg.LogName == "System.Logging.LoggingMonitor"; // 只写LoggingMonitor日志
                };
            });
        }
        if (App.GetConfig<bool>("Logging:ElasticSearch:Enabled")) // 日志写入ElasticSearch
        {
            services.AddDatabaseLogging<ElasticSearchLoggingWriter>(options =>
            {
                options.WithStackFrame = true; // 显示堆栈信息
                options.WithTraceId = true; // 显示线程Id
                options.IgnoreReferenceLoop = false; // 忽略循环检测
                options.MessageFormat = LoggerFormatter.Json;
                options.WriteFilter = (logMsg) =>
                {
                    return logMsg.LogName == "System.Logging.LoggingMonitor"; // 只写LoggingMonitor日志
                };
            });
        }

        // 雪花Id
        YitIdHelper.SetIdGenerator(App.GetOptions<SnowIdOptions>());

        // 验证码
        services.AddLazyCaptcha();
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

        //// 启用HTTPS
        //app.UseHttpsRedirection();

        // 特定文件类型（文件后缀）处理
        var contentTypeProvider = FS.GetFileExtensionContentTypeProvider();
        // contentTypeProvider.Mappings[".文件后缀"] = "MIME 类型";
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = contentTypeProvider
        });

        app.UseRouting();

        app.UseCorsAccessor();

        // 限流组件（在跨域之后）
        app.UseIpRateLimiting();
        app.UseClientRateLimiting();

        app.UseAuthentication();
        app.UseAuthorization();

        // 任务调度看板
        app.UseScheduleUI();

        // 配置Swagger-Knife4UI（路由前缀一致代表独立，不同则代表共存）
        app.UseKnife4UI(options =>
        {
            options.RoutePrefix = "kapi";
            foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
            {
                options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
            }
        });

        app.UseInject(string.Empty);

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