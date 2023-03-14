namespace Admin.NET.Core.Service;

/// <summary>
/// 枚举值服务
/// </summary>
[ApiDescriptionSettings(Order = 1000)]
[AllowAnonymous]
public class SysEnumDataService : IDynamicApiController, ITransient
{
    private readonly EnumOptions _enumOptions;

    public SysEnumDataService(IOptions<EnumOptions> enumOptions)
    {
        _enumOptions = enumOptions.Value;
    }

    /// <summary>
    /// 获取所有枚举值
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<EnumTypeOutput> GetEnumTypeList()
    {

        List<EnumTypeOutput> result = new List<EnumTypeOutput>();
        var enumTypeList = App.EffectiveTypes.Where(t => t.IsEnum && _enumOptions.EntityAssemblyNames.Contains(t.Assembly.GetName().Name)).ToList();

        foreach (var item in enumTypeList)
        {
            result.Add(GetEnumDescription(item));
        }
        return result;
    }

    private EnumTypeOutput GetEnumDescription(Type type)
    {

        string description = type.Name;
        var attrs = type.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attrs.Any())
        {
            //获取到：超级管理员
            var att = ((DescriptionAttribute[])attrs)[0];
            description = att.Description;
        }
        return new EnumTypeOutput
        {
            TypeDescribe = description,
            TypeName = type.Name,
            TypeRemark = description
        };
    }

    /// <summary>
    /// 通过枚举类型获取枚举值集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public List<EnumEntity> GetEnumDataList([FromQuery] EnumDataInput input)
    {
        // 查找枚举
        var enumType = App.EffectiveTypes.FirstOrDefault(t => t.IsEnum && t.Name == input.EnumName);
        if (enumType is not { IsEnum: true })
            throw Oops.Oh(ErrorCodeEnum.D1503);

        return enumType.EnumToList();
    }

    /// <summary>
    /// 通过实体字段类型获取相关集合（目前仅支持枚举类型）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public List<EnumEntity> GetEnumDataListByField([FromQuery] QueryEnumDataInput input)
    {

        // 获取实体类型属性
        Type entityType = App.EffectiveTypes.FirstOrDefault(t =>t.Name == input.EntityName) ?? throw Oops.Oh(ErrorCodeEnum.D1504);

        // 获取字段类型
        var fieldType = entityType.GetProperties().FirstOrDefault(p => p.Name == input.FieldName)?.PropertyType;
        if (fieldType is not { IsEnum: true })
            throw Oops.Oh(ErrorCodeEnum.D1503);

        return fieldType.EnumToList();
    }
}
