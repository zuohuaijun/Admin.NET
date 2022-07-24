using Admin.NET.Application.Const;
using Furion.DatabaseAccessor;
using Furion.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Admin.NET.Application.Serice;

/// <summary>
/// 自己业务服务
/// </summary>
[ApiDescriptionSettings(TestConst.GroupName, Name = "XXX模块", Order = 200)]
[AllowAnonymous]
[LoggingMonitor]
public class TestService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<Test> _testRep;

    public TestService(SqlSugarRepository<Test> testRep)
    {
        _testRep = testRep;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test/list")]
    public async Task<List<Test>> GetTestList()
    {
        return await _testRep.GetListAsync();
    }

    /// <summary>
    /// 事务和工作单元测试
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test/list2")]
    [UnitOfWork]
    public async Task<List<Test>> TestUnitOfWork()
    {
        await _testRep.InsertAsync(new Test() { Name = "admin" });
        throw new Exception("异常");
        return await _testRep.GetListAsync();
    }
}