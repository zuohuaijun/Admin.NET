namespace Admin.NET.Application.Service;

/// <summary>
/// 测试服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 200)]
[AllowAnonymous]
public class TestService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Test> _testRep;

    public TestService(SqlSugarRepository<Test> testRep)
    {
        _testRep = testRep;
    }

    /// <summary>
    /// 接口测试
    /// </summary>
    public string GetName()
    {
        return "Furion";
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    /// <returns></returns>
    public async Task<bool> AddTest()
    {
        return await _testRep.InsertAsync(new Test() { Name = "王五" });
    }

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test/list")]
    public async Task<List<Test>> GetTestList()
    {
        return await _testRep.GetListAsync();
    }
}