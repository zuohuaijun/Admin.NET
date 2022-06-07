namespace Admin.NET.Core;

/// <summary>
/// 代码生成配置选项
/// </summary>
public class CodeGenOptions : IConfigurableOptions
{
    /// <summary>
    /// 数据库实体程序集名称集合
    /// </summary>
    public List<string> EntityAssemblyNames { get; set; }
}