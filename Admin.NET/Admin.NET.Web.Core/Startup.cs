using Admin.NET.Core;
using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnceMi.AspNetCore.OSS;
using Serilog;
using Yitter.IdGenerator;

namespace Admin.NET.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurableOptions<ConnectionStringsOptions>();
            services.AddConfigurableOptions<RefreshTokenOptions>();
            services.AddConfigurableOptions<SnowIdOptions>();
            services.AddConfigurableOptions<CacheOptions>();
            services.AddConfigurableOptions<OSSProviderOptions>();
            services.AddConfigurableOptions<UploadOptions>();
            services.AddConfigurableOptions<WechatOptions>();
            services.AddConfigurableOptions<WechatPayOptions>();
            services.AddConfigurableOptions<PayCallBackOptions>();
            services.AddConfigurableOptions<CodeGenOptions>();

            services.AddSqlSugarSetup(App.Configuration);

            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

            services.AddCorsAccessor();
            services.AddRemoteRequest();

            services.AddTaskScheduler();

            services.AddControllersWithViews()
                .AddMvcFilter<RequestActionFilter>()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 响应驼峰命名
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
                })
                .AddInjectWithUnifyResult<AdminResultProvider>();

            // 注册日志事件订阅者(支持自定义消息队列组件)
            services.AddEventBus(builder =>
            {
                builder.AddSubscriber<LogEventSubscriber>();
            });

            // 注册OSS对象存储
            services.AddOSSService(option =>
            {
                var ossOptions = App.GetOptions<OSSProviderOptions>();
                option.Provider = (OSSProvider)ossOptions.Provider;
                option.Endpoint = ossOptions.Endpoint;
                option.AccessKey = ossOptions.AccessKey;
                option.SecretKey = ossOptions.SecretKey;
                option.Region = ossOptions.Region;
                option.IsEnableCache = ossOptions.IsEnableCache;
                option.IsEnableHttps = ossOptions.IsEnableHttps;
            });

            // 注册CSRedis缓存
            services.AddCSRedisSetup();

            // 注册模板引擎
            services.AddViewEngine();

            // 注册即时通讯
            services.AddSignalR();

            // 增加Logo输出显示
            services.AddLogoDisplay();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Serilog请求日志中间件---必须在 UseStaticFiles 和 UseRouting 之间
            app.UseSerilogRequestLogging();

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
}