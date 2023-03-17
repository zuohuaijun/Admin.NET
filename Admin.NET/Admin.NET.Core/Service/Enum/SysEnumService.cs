namespace Admin.NET.Core.Service;

/// <summary>
/// 系统枚举服务
/// </summary>
[ApiDescriptionSettings(Order = 275)]
[AllowAnonymous]
public class SysEnumService : IDynamicApiController, ITransient
{
    private readonly EnumOptions _enumOptions;

    public SysEnumService(IOptions<EnumOptions> enumOptions)
    {
        _enumOptions = enumOptions.Value;
    }

    /// <summary>
    /// 获取所有枚举类型
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取所有枚举类型")]
    public List<EnumTypeOutput> GetEnumTypeList()
    {
        var enumTypeList = App.EffectiveTypes.Where(t => t.IsEnum && _enumOptions.EntityAssemblyNames.Contains(t.Assembly.GetName().Name)).ToList();

        var result = new List<EnumTypeOutput>();
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
    [DisplayName("通过枚举类型获取枚举值集合")]
    public List<EnumEntity> GetEnumDataList([FromQuery] EnumInput input)
    {
        var enumType = App.EffectiveTypes.FirstOrDefault(t => t.IsEnum && t.Name == input.EnumName);
        if (enumType is not { IsEnum: true })
            throw Oops.Oh(ErrorCodeEnum.D1503);

        return enumType.EnumToList();
    }

    /// <summary>
    /// 通过实体的字段名获取相关枚举值集合（目前仅支持枚举类型）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("通过实体的字段名获取相关枚举值集合")]
    public List<EnumEntity> GetEnumDataListByField([FromQuery] QueryEnumDataInput input)
    {
        // 获取实体类型属性
        Type entityType = App.EffectiveTypes.FirstOrDefault(t => t.Name == input.EntityName) ?? throw Oops.Oh(ErrorCodeEnum.D1504);

        // 获取字段类型
        var fieldType = entityType.GetProperties().FirstOrDefault(p => p.Name == input.FieldName)?.PropertyType;
        if (fieldType is not { IsEnum: true })
            throw Oops.Oh(ErrorCodeEnum.D1503);

        return fieldType.EnumToList();
    }
}