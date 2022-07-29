using Admin.NET.Application.Serice;
using Xunit;

namespace Admin.NET.UnitTest;

/// <summary>
/// 单元测试
/// </summary>
public class UnitTest2
{
    private readonly TestService _testService;

    public UnitTest2(TestService testService)
    {
        _testService = testService;
    }

    [Fact]
    public void 测试接口()
    {
        var res = _testService.GetName();
        Assert.Equal("Furion1", res);
    }
}