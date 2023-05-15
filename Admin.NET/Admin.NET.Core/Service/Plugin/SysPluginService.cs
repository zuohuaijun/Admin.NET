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
    public async Task AddPos(AddPluginInput input)
    {
        var isExist = await _sysPluginRep.IsAnyAsync(u => u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1900);

        await _sysPluginRep.InsertAsync(input.Adapt<SysPlugin>());
    }

    /// <summary>
    /// 更新动态插件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新动态插件")]
    public async Task UpdatePos(UpdatePluginInput input)
    {
        var isExist = await _sysPluginRep.IsAnyAsync(u => u.Name == input.Name && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1900);

        await _sysPluginRep.AsUpdateable(input.Adapt<SysPlugin>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除动态插件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除动态插件")]
    public async Task DeletePos(DeletePluginInput input)
    {
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