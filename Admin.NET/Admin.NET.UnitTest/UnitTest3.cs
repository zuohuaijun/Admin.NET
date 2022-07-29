//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Threading.Tasks;
//using Xunit;

//namespace Admin.NET.UnitTest;

///// <summary>
///// 集成测试
///// </summary>
//public class UnitTest3 : IClassFixture<WebApplicationFactory<Admin.NET.Web.Entry.FakeStartup>>
//{
//    private readonly WebApplicationFactory<Admin.NET.Web.Entry.FakeStartup> _factory;

//    public UnitTest2(WebApplicationFactory<Admin.NET.Web.Entry.FakeStartup> factory)
//    {
//        _factory = factory;
//    }

//    [Theory]
//    [InlineData("/test/get")]
//    public async Task 测试接口2(string url)
//    {
//        using var client = _factory.CreateClient();
//        using var response = await client.GetAsync(url);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadAsStringAsync();
//        Assert.Contains("Furion", result);
//    }
//}