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
/// 系统动态插件服务
/// </summary>
[ApiDescriptionSettings(Order = 245)]
public class SysPluginService : IDynamicApiController, ITransient
{
    private readonly IDynamicApiRuntimeChangeProvider _provider;
    private readonly SqlSugarRepository<SysPlugin> _sysPluginRep;

    public SysPluginService(IDynamicApiRuntimeChangeProvider provider,
        SqlSugarRepository<SysPlugin> sysPluginRep)
    {
        _provider = provider;
        _sysPluginRep = sysPluginRep;
    }

    /// <summary>
    /// 获取动态插件列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取动态插件列表")]
    public async Task<SqlSugarPagedList<SysPlugin>> Page(PagePluginInput input)
    {
        return await _sysPluginRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加动态插件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加动态插件")]
    public async Task AddPlugin(AddPluginInput input)
    {
        var isExist = await _sysPluginRep.IsAnyAsync(u => u.Name == input.Name || u.AssemblyName == input.AssemblyName);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1900);

        // 添加动态程序集/接口
        input.AssemblyName = CompileAssembly(input.CsharpCode, input.AssemblyName);

        await _sysPluginRep.InsertAsync(input.Adapt<SysPlugin>());
    }

    /// <summary>
    /// 更新动态插件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新动态插件")]
    public async Task UpdatePlugin(UpdatePluginInput input)
    {
        var isExist = await _sysPluginRep.IsAnyAsync(u => (u.Name == input.Name || u.AssemblyName == input.AssemblyName) && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1900);

        // 先移除再添加动态程序集/接口
        RemoveAssembly(input.AssemblyName);
        input.AssemblyName = CompileAssembly(input.CsharpCode);

        await _sysPluginRep.AsUpdateable(input.Adapt<SysPlugin>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除动态插件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除动态插件")]
    public async Task DeletePlugin(DeletePluginInput input)
    {
        var plugin = await _sysPluginRep.GetByIdAsync(input.Id);
        if (plugin == null) return;

        // 移除动态程序集/接口
        RemoveAssembly(plugin.AssemblyName);

        await _sysPluginRep.DeleteAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 添加动态程序集/接口
    /// </summary>
    /// <param name="csharpCode"></param>
    /// <param name="assemblyName">程序集名称</param>
    /// <returns></returns>
    [DisplayName("添加动态程序集/接口")]
    public string CompileAssembly([FromBody] string csharpCode, [FromQuery] string assemblyName = default)
    {
        // 编译 C# 代码并返回动态程序集
        var dynamicAssembly = App.CompileCSharpClassCode(csharpCode, assemblyName);

        // 将程序集添加进动态 WebAPI 应用部件
        _provider.AddAssembliesWithNotifyChanges(dynamicAssembly);

        // 返回动态程序集名称
        return dynamicAssembly.GetName().Name;
    }

    /// <summary>
    /// 移除动态程序集/接口
    /// </summary>
    [ApiDescriptionSettings(Name = "RemoveAssembly"), HttpPost]
    [DisplayName("移除动态程序集/接口")]
    public void RemoveAssembly(string assemblyName)
    {
        _provider.RemoveAssembliesWithNotifyChanges(assemblyName);
    }
}