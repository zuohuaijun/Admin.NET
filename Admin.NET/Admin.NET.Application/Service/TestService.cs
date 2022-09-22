using Admin.NET.Application.Const;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Furion.Localization;
using Furion.Logging.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Admin.NET.Application.Service;

/// <summary>
/// 自己业务服务
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
    /// 测试
    /// </summary>
    public string GetName()
    {
        return "Furion";
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
    /// 异常测试
    /// </summary>
    /// <returns></returns>
    public void TestException()
    {
        throw new Exception("异常");

        throw Oops.Oh("异常").WithData("数据");
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
        var a = 1;
        var b = 0;
        var c = a / b;
        return await _testRep.GetListAsync();
    }

    /// <summary>
    /// 多语言测试
    /// </summary>
    /// <returns></returns>
    public string TestCulture()
    {
        "ddd".LogWarning();
        //L.SetCulture("zh-CN");
        //var a = L.GetSelectCulture();
        //var a1 = L.Text["API Interfaces"];
        //return $"当前语言【{a.Culture.Name}】 {a1}";

        L.SetCulture("en-US");
        var b = L.GetSelectCulture();
        var b1 = L.Text["API 接口"];

        return $"当前语言【{b.Culture.Name}】 {b1}";
    }

    /// <summary>
    /// 自定义规范化结果
    /// </summary>
    /// <returns></returns>
    [CustomUnifyResult("APP")]
    public string CustomUnifyResult()
    {
        return "Furion";
    }
}