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
/// 系统打印模板服务
/// </summary>
[ApiDescriptionSettings(Order = 305)]
public class SysPrintService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysPrint> _sysPrintRep;

    public SysPrintService(SqlSugarRepository<SysPrint> sysPrintRep)
    {
        _sysPrintRep = sysPrintRep;
    }

    /// <summary>
    /// 获取打印模板列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取打印模板列表")]
    public async Task<SqlSugarPagedList<SysPrint>> Page(PagePrintInput input)
    {
        return await _sysPrintRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加打印模板")]
    public async Task AddPrint(AddPrintInput input)
    {
        var isExist = await _sysPrintRep.IsAnyAsync(u => u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1800);

        await _sysPrintRep.InsertAsync(input.Adapt<SysPrint>());
    }

    /// <summary>
    /// 更新打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新打印模板")]
    public async Task UpdatePrint(UpdatePrintInput input)
    {
        var isExist = await _sysPrintRep.IsAnyAsync(u => u.Name == input.Name && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1800);

        await _sysPrintRep.AsUpdateable(input.Adapt<SysPrint>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除打印模板")]
    public async Task DeletePrint(DeletePrintInput input)
    {
        await _sysPrintRep.DeleteAsync(u => u.Id == input.Id);
    }
}