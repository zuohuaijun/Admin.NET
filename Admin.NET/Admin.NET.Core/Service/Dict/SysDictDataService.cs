// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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