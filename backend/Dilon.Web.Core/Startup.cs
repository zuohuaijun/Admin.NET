using Dilon.Core;
using Furion;
using Furion.Snowflake;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text.Json;

namespace Dilon.Web.Core
{
    [AppStartup(100)]
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);

            services.AddCorsAccessor();

            services.AddRemoteRequest();

            services.AddConfigurableOptions<CacheOptions>();

            services.AddControllersWithViews()
                    .AddMvcFilter<RequestActionFilter>()
                    .AddInjectWithUnifyResult<XnRestfulResultProvider>()
                    .AddJsonOptions(options =>
                    {
                        //options.JsonSerializerOptions.DefaultBufferSize = 10_0000;//返回较大数据数据序列化时会截断，原因：默认缓冲区大小（以字节为单位）为16384。
                        options.JsonSerializerOptions.Converters.AddDateFormatString("yyyy-MM-dd HH:mm:ss");
                        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // 忽略循环引用 仅.NET 6支持
                    });

            services.AddViewEngine();

            // 设置雪花id的workerId，确保每个实例workerId都应不同
            var workerId = ushort.Parse(App.Configuration["SnowId:WorkerId"] ?? "1");
            IDGenerator.SetIdGenerator(new IDGeneratorOptions { WorkerId = workerId });
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

            app.UseHttpsRedirection(); // 强制https
            app.UseStaticFiles();

            // Serilog请求日志中间件---必须在 UseStaticFiles 和 UseRouting 之间
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}