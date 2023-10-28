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