using Xunit;
using Xunit.Abstractions;

namespace Admin.NET.UnitTest;

/// <summary>
/// 单元测试
/// </summary>
public class UnitTest1
{
    private readonly ITestOutputHelper _output; // 日志记录

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test1()
    {
        Assert.Equal(2, 1 + 1);
    }

    [Fact]
    public void 测试日志()
    {
        _output.WriteLine("哈哈哈哈，我是 Furion");
        Assert.NotEqual("Furion", "Fur");
    }

    /// <summary>
    /// 带参数（奇数判断）
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    [InlineData(5, 7)]
    public void 带参数测试(int i, int j)
    {
        Assert.NotEqual(0, (i + j) % 2);
    }

    //[Fact]
    //public async Task 测试请求百度()
    //{
    //    var rep = await "https://www.baidu.com".GetAsync();
    //    Assert.True(rep.IsSuccessStatusCode);
    //}
}