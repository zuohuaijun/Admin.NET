namespace Admin.NET.Core.Service;

/// <summary>
/// 系统常量服务
/// </summary>
[ApiDescriptionSettings(Order = 189)]
[AllowAnonymous]
public class SysConstService : IDynamicApiController, ITransient
{
    private readonly ISysCacheService _sysCacheService;

    public SysConstService(ISysCacheService sysCacheService)
    {
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取所有常量下拉框列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/constSelector/allConstSelector")]
    public async Task<List<SelectorDto>> GetAllConstSelector()
    {
        var key = $"{CacheConst.KeyConstSelector}AllSelector";
        var json = await _sysCacheService.GetStringAsync(key);
        if (!string.IsNullOrWhiteSpace(json))
        {
            return json.ToObject<List<SelectorDto>>();
        }
        var typeList = await GetAllTypesAsync();
        var selectData = typeList.Select(x => new SelectorDto
        {
            Name = x.CustomAttributes.ToList().FirstOrDefault()?.ConstructorArguments.ToList().FirstOrDefault().Value?.ToString() ?? x.Name,
            Code = x.Name
        }).ToList();
        await _sysCacheService.SetStringAsync(key, selectData.ToJson());
        return selectData;
    }

    /// <summary>
    /// 根据类名获取下拉框数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    [HttpGet("/constSelector/constSelector")]
    public async Task<List<SelectorDto>> GetConstSelector(string typeName)
    {
        var key = $"{CacheConst.KeyConstSelector}{typeName.ToUpper()}";
        var json = await _sysCacheService.GetStringAsync(key);
        if (!string.IsNullOrWhiteSpace(json))
        {
            return json.ToObject<List<SelectorDto>>();
        }
        var typeList = await GetAllTypesAsync();
        var type = typeList.FirstOrDefault(x => x.Name == typeName);

        var isEnum = type.BaseType.Name == "Enum";
        var selectData = type.GetFields()?
            .Where(isEnum, x => x.FieldType.Name == typeName)
            .Select(x => new SelectorDto
            {
                Name = x.Name,
                Code = isEnum ? (int)x.GetValue(BindingFlags.Instance) : x.GetValue(BindingFlags.Instance)
            }).ToList();
        await _sysCacheService.SetStringAsync(key, selectData.ToJson());
        return selectData;
    }

    /// <summary>
    /// 获取所有下拉框及选项  用于前端缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet("/constSelector/allConstSelectorWithOptions")]
    public async Task<List<SelectorDto>> GetAllConstSelectorWithOptions()
    {
        var selectors = await GetAllConstSelector();
        foreach (var p in selectors)
        {
            p.Data = await GetConstSelector(Convert.ToString(p.Code));
        }
        return selectors;
    }

    /// <summary>
    /// 获取所有常量
    /// </summary>
    /// <returns></returns>
    private async Task<List<Type>> GetAllTypesAsync()
    {
        return await Task.Run(() =>
        {
            var typeList = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ConstSelectorAttribute)))
                .ToList();
            return typeList;
        });
    }
}