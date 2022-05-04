using Admin.NET.Core;
using Furion;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

// 配置启动类类型，第一个参数是 Startup 类完整限定名，第二个参数是当前项目程序集名称
[assembly: TestFramework("Admin.NET.UnitTest.Startup", "Admin.NET.UnitTest")]

namespace Admin.NET.UnitTest
{
    /// <summary>
    /// 单元测试启动类
    /// </summary>
    /// <remarks>在这里可以使用 Furion 几乎所有功能</remarks>
    public sealed class Startup : XunitTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink)
        {
            // 初始化 IServiceCollection 对象
            var services = Inject.Create();

            // 在这里可以和 .NET Core 一样注册服务了！！！

            services.AddScoped<IUserManager, TestUserManager>();

            // 构建 ServiceProvider 对象
            services.Build();
        }
    }
}