// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统字典类型服务
/// </summary>
[ApiDescriptionSettings(Order = 430)]
[AllowAnonymous]
public class SysDictTypeService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysDictType> _sysDictTypeRep;
    private readonly SysDictDataService _sysDictDataService;

    public SysDictTypeService(SqlSugarRepository<SysDictType> sysDictTypeRep,
        SysDictDataService sysDictDataService)
    {
        _sysDictTypeRep = sysDictTypeRep;
        _sysDictDataService = sysDictDataService;
    }

    /// <summary>
    /// 获取字典类型分页列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取字典类型分页列表")]
    public async Task<SqlSugarPagedList<SysDictType>> Page(PageDictTypeInput input)
    {
        return await _sysDictTypeRep.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.Code?.Trim()), u => u.Code.Contains(input.Code))
            .WhereIF(!string.IsNullOrEmpty(input.Name?.Trim()), u => u.Name.Contains(input.Name))
            .OrderBy(u => new { u.OrderNo, u.Code })
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取字典类型列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取字典类型列表")]
    public async Task<List<SysDictType>> GetList()
    {
        return await _sysDictTypeRep.AsQueryable().OrderBy(u => new { u.OrderNo, u.Code }).ToListAsync();
    }

    /// <summary>
    /// 获取字典类型-值列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("获取字典类型-值列表")]
    public async Task<List<SysDictData>> GetDataList([FromQuery] GetDataDictTypeInput input)
    {
        var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Code == input.Code);
        if (dictType == null)
            throw Oops.Oh(ErrorCodeEnum.D3000);

        return await _sysDictDataService.GetDictDataListByDictTypeId(dictType.Id);
    }

    /// <summary>
    /// 添加字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("添加字典类型")]
    public async Task AddDictType(AddDictTypeInput input)
    {
        var isExist = await _sysDictTypeRep.IsAnyAsync(u => u.Code == input.Code);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D3001);

        await _sysDictTypeRep.InsertAsync(input.Adapt<SysDictType>());
    }

    /// <summary>
    /// 更新字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新字典类型")]
    public async Task UpdateDictType(UpdateDictTypeInput input)
    {
        var isExist = await _sysDictTypeRep.IsAnyAsync(u => u.Id == input.Id);
        if (!isExist)
            throw Oops.Oh(ErrorCodeEnum.D3000);

        isExist = await _sysDictTypeRep.IsAnyAsync(u => u.Code == input.Code && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D3001);

        await _sysDictTypeRep.UpdateAsync(input.Adapt<SysDictType>());
    }

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除字典类型")]
    public async Task DeleteDictType(DeleteDictTypeInput input)
    {
        var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
        if (dictType == null)
            throw Oops.Oh(ErrorCodeEnum.D3000);

        // 删除字典值
        await _sysDictTypeRep.DeleteAsync(dictType);
        await _sysDictDataService.DeleteDictData(input.Id);
    }

    /// <summary>
    /// 获取字典类型详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取字典类型详情")]
    public async Task<SysDictType> GetDetail([FromQuery] DictTypeInput input)
    {
        return await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 修改字典类型状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("修改字典类型状态")]
    public async Task SetStatus(DictTypeInput input)
    {
        var dictType = await _sysDictTypeRep.GetFirstAsync(u => u.Id == input.Id);
        if (dictType == null)
            throw Oops.Oh(ErrorCodeEnum.D3000);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        dictType.Status = input.Status;
        await _sysDictTypeRep.UpdateAsync(dictType);
    }

    /// <summary>
    /// 获取所有字典集合
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("获取所有字典集合")]
    public async Task<List<SysDictType>> GetAllDictList()
    {
        var dictList = await _sysDictTypeRep.AsQueryable()
            .Includes(u => u.Children)
            .OrderBy(u => new { u.OrderNo, u.Code })
            .ToListAsync();
        // 字典数据项排序
        dictList.ForEach(u => u.Children = u.Children.OrderBy(c => c.OrderNo).ThenBy(c => c.Code).ToList());
        return dictList;
    }
}