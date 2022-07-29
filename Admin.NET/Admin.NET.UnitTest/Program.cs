using Furion.Xunit;
using System;
using Xunit;
using Xunit.Abstractions;

// 配置启动类类型，第一个参数是 Startup 类完整限定名，第二个参数是当前项目程序集名称
[assembly: TestFramework("Admin.NET.UnitTest.Program", "Admin.NET.UnitTest")]

namespace Admin.NET.UnitTest;

/// <summary>
/// 单元测试启动类
/// </summary>
internal class Program : TestStartup
{
    public Program(IMessageSink messageSink) : base(messageSink)
    {
        // 初始化 Furion
        Serve.Run(silence: true);
    }
}