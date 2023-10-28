// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统字典值服务
/// </summary>
[ApiDescriptionSettings(Order = 420)]
[AllowAnonymous]
public class SysDictDataService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysDictData> _sysDictDataRep;

    public SysDictDataService(SqlSugarRepository<SysDictData> sysDictDataRep)
    {
        _sysDictDataRep = sysDictDataRep;
    }

    /// <summary>
    /// 获取字典值分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取字典值分页列表")]
    public async Task<SqlSugarPagedList<SysDictData>> Page(PageDictDataInput input)
    {
        return await _sysDictDataRep.AsQueryable()
            .Where(u => u.DictTypeId == input.DictTypeId)
            .WhereIF(!string.IsNullOrEmpty(input.Code?.Trim()), u => u.Code.Contains(input.Code))
            .WhereIF(!string.IsNullOrEmpty(input.Value?.Trim()), u => u.Value.Contains(input.Value))
            .OrderBy(u => new { u.OrderNo, u.Code })
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取字典值列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取字典值列表")]
    public async Task<List<SysDictData>> GetList([FromQuery] GetDataDictDataInput input)
    {
        return await GetDictDataListByDictTypeId(input.DictTypeId);
    }

    /// <summary>
    /// 增加字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加字典值")]
    public async Task AddDictData(AddDictDataInput input)
    {
        var isExist = await _sysDictDataRep.IsAnyAsync(u => u.Code == input.Code && u.DictTypeId == input.DictTypeId);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D3003);

        await _sysDictDataRep.InsertAsync(input.Adapt<SysDictData>());
    }

    /// <summary>
    /// 更新字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新字典值")]
    public async Task UpdateDictData(UpdateDictDataInput input)
    {
        var isExist = await _sysDictDataRep.IsAnyAsync(u => u.Id == input.Id);
        if (!isExist) throw Oops.Oh(ErrorCodeEnum.D3004);

        isExist = await _sysDictDataRep.IsAnyAsync(u => u.Code == input.Code && u.DictTypeId == input.DictTypeId && u.Id != input.Id);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D3003);

        await _sysDictDataRep.UpdateAsync(input.Adapt<SysDictData>());
    }

    /// <summary>
    /// 删除字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除字典值")]
    public async Task DeleteDictData(DeleteDictDataInput input)
    {
        var dictData = await _sysDictDataRep.GetFirstAsync(u => u.Id == input.Id);
        if (dictData == null)
            throw Oops.Oh(ErrorCodeEnum.D3004);

        await _sysDictDataRep.DeleteAsync(dictData);
    }

    /// <summary>
    /// 获取字典值详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取字典值详情")]
    public async Task<SysDictData> GetDetail([FromQuery] DictDataInput input)
    {
        return await _sysDictDataRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 修改字典值状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("修改字典值状态")]
    public async Task SetStatus(DictDataInput input)
    {
        var dictData = await _sysDictDataRep.GetFirstAsync(u => u.Id == input.Id);
        if (dictData == null)
            throw Oops.Oh(ErrorCodeEnum.D3004);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        dictData.Status = input.Status;
        await _sysDictDataRep.UpdateAsync(dictData);
    }

    /// <summary>
    /// 根据字典类型Id获取字典值集合
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<SysDictData>> GetDictDataListByDictTypeId(long dictTypeId)
    {
        return await _sysDictDataRep.AsQueryable()
            .Where(u => u.DictTypeId == dictTypeId)
            .OrderBy(u => new { u.OrderNo, u.Code })
            .ToListAsync();
    }

    /// <summary>
    /// 根据字典类型编码获取字典值集合
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [DisplayName("根据字典类型编码获取字典值集合")]
    public async Task<List<SysDictData>> GetDataList(string code)
    {
        return await _sysDictDataRep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((u, a) => u.Id == a.DictTypeId)
            .Where((u, a) => u.Code == code && u.Status == StatusEnum.Enable && a.Status == StatusEnum.Enable)
            .OrderBy((u, a) => new { a.OrderNo, a.Code })
            .Select((u, a) => a).ToListAsync();
    }

    /// <summary>
    /// 根据查询条件获取字典值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("根据查询条件获取字典值集合")]
    public async Task<List<SysDictData>> GetDataList([FromQuery] QueryDictDataInput input)
    {
        return await _sysDictDataRep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((u, a) => u.Id == a.DictTypeId)
            .Where((u, a) => u.Code == input.Code)
            .WhereIF(input.Status.HasValue, (u, a) => a.Status == (StatusEnum)input.Status.Value)
            .OrderBy((u, a) => new { a.OrderNo, a.Code })
            .Select((u, a) => a).ToListAsync();
    }

    /// <summary>
    /// 根据字典类型Id删除字典值
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task DeleteDictData(long dictTypeId)
    {
        await _sysDictDataRep.DeleteAsync(u => u.DictTypeId == dictTypeId);
    }
}