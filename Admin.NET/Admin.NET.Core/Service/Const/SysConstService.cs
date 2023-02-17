namespace Admin.NET.Core.Service;

/// <summary>
/// 系统常量服务
/// </summary>
[ApiDescriptionSettings(Order = 280)]
[AllowAnonymous]
public class SysConstService : IDynamicApiController, ITransient
{
    private readonly SysCacheService _sysCacheService;

    public SysConstService(SysCacheService sysCacheService)
    {
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取所有常量列表
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "List")]
    [DisplayName("获取所有常量列表")]
    public async Task<List<ConstOutput>> GetList()
    {
        var key = $"{CacheConst.KeyConst}list";
        var constlist = _sysCacheService.Get<List<ConstOutput>>(key);
        if (constlist == null)
        {
            var typeList = GetConstAttributeList();
            constlist = typeList.Select(x => new ConstOutput
            {
                Name = x.CustomAttributes.ToList().FirstOrDefault()?.ConstructorArguments.ToList().FirstOrDefault().Value?.ToString() ?? x.Name,
                Code = x.Name,
                Data = GetData(Convert.ToString(x.Name))
            }).ToList();
            _sysCacheService.Set(key, constlist);
        }
        return await Task.FromResult(constlist);
    }

    /// <summary>
    /// 根据类名获取常量数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Data")]
    [DisplayName("根据类名获取常量数据")]
    public async Task<List<ConstOutput>> GetData([Required] string typeName)
    {
        var key = $"{CacheConst.KeyConst}{typeName.ToUpper()}";
        var constlist = _sysCacheService.Get<List<ConstOutput>>(key);
        if (constlist == null)
        {
            var typeList = GetConstAttributeList();
            var type = typeList.FirstOrDefault(x => x.Name == typeName);

            var isEnum = type.BaseType.Name == "Enum";
            constlist = type.GetFields()?
                .Where(isEnum, x => x.FieldType.Name == typeName)
                .Select(x => new ConstOutput
                {
                    Name = x.Name,
                    Code = isEnum ? (int)x.GetValue(BindingFlags.Instance) : x.GetValue(BindingFlags.Instance)
                }).ToList();
            _sysCacheService.Set(key, constlist);
        }
        return await Task.FromResult(constlist);
    }

    /// <summary>
    /// 获取常量特性类型列表
    /// </summary>
    /// <returns></returns>
    private List<Type> GetConstAttributeList()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ConstAttribute))).ToList();
    }
}