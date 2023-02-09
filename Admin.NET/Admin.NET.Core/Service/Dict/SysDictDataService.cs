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
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<SysDictData>> GetPage([FromQuery] PageDictDataInput input)
    {
        var code = !string.IsNullOrEmpty(input.Code?.Trim());
        var value = !string.IsNullOrEmpty(input.Value?.Trim());
        return await _sysDictDataRep.AsQueryable()
            .Where(u => u.DictTypeId == input.DictTypeId)
            .WhereIF(code, u => u.Code.Contains(input.Code))
            .WhereIF(value, u => u.Value.Contains(input.Value))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取字典值列表
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<SysDictData>> GetList([FromQuery] GetDataDictDataInput input)
    {
        return await GetDictDataListByDictTypeId(input.DictTypeId);
    }

    /// <summary>
    /// 增加字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add")]
    public async Task AddDictData(AddDictDataInput input)
    {
        var isExist = await _sysDictDataRep.IsAnyAsync(u => (u.Code == input.Code || u.Value == input.Value) && u.DictTypeId == input.DictTypeId);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D3003);

        await _sysDictDataRep.InsertAsync(input.Adapt<SysDictData>());
    }

    /// <summary>
    /// 更新字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update")]
    public async Task UpdateDictData(UpdateDictDataInput input)
    {
        var isExist = await _sysDictDataRep.IsAnyAsync(u => u.Id == input.Id);
        if (!isExist) throw Oops.Oh(ErrorCodeEnum.D3004);

        isExist = await _sysDictDataRep.IsAnyAsync(u => (u.Value == input.Value || u.Code == input.Code) && u.DictTypeId == input.DictTypeId && u.Id != input.Id);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D3003);

        await _sysDictDataRep.UpdateAsync(input.Adapt<SysDictData>());
    }

    /// <summary>
    /// 删除字典值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete")]
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
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<SysDictData> GetDetail([FromQuery] DictDataInput input)
    {
        return await _sysDictDataRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 修改字典值状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SetStatus")]
    public async Task SetStatus(DictDataInput input)
    {
        var dictData = await _sysDictDataRep.GetFirstAsync(u => u.Id == input.Id);
        if (dictData == null)
            throw Oops.Oh(ErrorCodeEnum.D3004);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        dictData.Status = (StatusEnum)input.Status;
        await _sysDictDataRep.UpdateAsync(dictData);
    }

    /// <summary>
    /// 根据字典类型Id获取字典值集合
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public async Task<List<SysDictData>> GetDictDataListByDictTypeId(long dictTypeId)
    {
        return await _sysDictDataRep.AsQueryable()
            .Where(u => u.DictTypeId == dictTypeId)
            .OrderBy(u => u.OrderNo).ToListAsync();
    }

    /// <summary>
    /// 根据字典类型编码获取字典值集合
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DataList")]
    public async Task<dynamic> GetDataList([Required] string code)
    {
        return await _sysDictDataRep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((a, b) => a.Id == b.DictTypeId)
            .Where((a, b) => a.Code == code && a.Status == StatusEnum.Enable && b.Status == StatusEnum.Enable)
            .Select((a, b) => new
            {
                Label = b.Value,
                Value = b.Code
            }).ToListAsync();
    }

    /// <summary>
    /// 根据查询条件获取字典值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DataList")]
    public async Task<dynamic> GetDataList([FromQuery] QueryDictDataInput input)
    {
        return await _sysDictDataRep.Context.Queryable<SysDictType>()
            .LeftJoin<SysDictData>((a, b) => a.Id == b.DictTypeId)
            .Where((a, b) => a.Code == input.Code)
            .WhereIF(input.Status.HasValue, (a, b) => b.Status == (StatusEnum)input.Status.Value)
            .Select((a, b) => new
            {
                Label = b.Value,
                Value = b.Code
            }).ToListAsync();
    }

    /// <summary>
    /// 根据字典类型Id删除字典值
    /// </summary>
    /// <param name="dictTypeId"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(false)]
    public async Task DeleteDictData(long dictTypeId)
    {
        await _sysDictDataRep.DeleteAsync(u => u.DictTypeId == dictTypeId);
    }
}