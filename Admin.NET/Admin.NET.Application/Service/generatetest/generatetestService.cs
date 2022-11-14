using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Mapster;

namespace Admin.NET.Application;
/// <summary>
/// GenerateTest服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class generatetestService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<generatetest> _rep;
    public generatetestService(SqlSugarRepository<generatetest> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询GenerateTest
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/generatetest/page")]
    public async Task<dynamic> Page(generatetestInput input)
    {
        var query= _rep.Context.Queryable<generatetest>()

                    .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
;

        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加GenerateTest
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/generatetest/add")]
    public async Task Add(AddgeneratetestInput input)
    {
        var entity = input.Adapt<generatetest>();
        await _rep.InsertWithDiffLogAsync(entity);
    }

    /// <summary>
    /// 删除GenerateTest
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/generatetest/delete")]
    public async Task Delete(DeletegeneratetestInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id);
        await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新GenerateTest
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/generatetest/edit")]
    public async Task Update(UpdategeneratetestInput input)
    {
        var entity = input.Adapt<generatetest>();
        await _rep.UpdateWithDiffLogAsync(entity);
    }

    /// <summary>
    /// 获取GenerateTest
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/generatetest/detail")]
    public async Task<generatetest> Get([FromQuery] QueryegeneratetestInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取GenerateTest列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/generatetest/list")]
    public async Task<dynamic> List([FromQuery] generatetestInput input)
    {
        return await _rep.AsQueryable().ToListAsync();
    }



}
