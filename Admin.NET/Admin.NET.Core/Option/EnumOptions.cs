namespace Admin.NET.Core;

/// <summary>
/// 枚举配置选项
/// </summary>
public sealed class EnumOptions : IConfigurableOptions
{
    /// <summary>
    /// 枚举实体程序集名称集合
    /// </summary>
    public List<string> EntityAssemblyNames { get; set; }
}