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
        var enumTypeList = App.EffectiveTypes.Where(u => u.IsEnum && _enumOptions.EntityAssemblyNames.Contains(u.Assembly.GetName().Name)).OrderBy( u => u.Name).ToList();

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
        var enumType = App.EffectiveTypes.FirstOrDefault(u => u.IsEnum && u.Name == input.EnumName);
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
        Type entityType = App.EffectiveTypes.FirstOrDefault(u => u.Name == input.EntityName) ?? throw Oops.Oh(ErrorCodeEnum.D1504);

        // 获取字段类型
        var fieldType = entityType.GetProperties().FirstOrDefault(u => u.Name == input.FieldName)?.PropertyType;
        if (fieldType is not { IsEnum: true })
            throw Oops.Oh(ErrorCodeEnum.D1503);

        return fieldType.EnumToList();
    }
}