// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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