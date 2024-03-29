// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 代码生成配置选项
/// </summary>
public sealed class CodeGenOptions : IConfigurableOptions
{
    /// <summary>
    /// 数据库实体程序集名称集合
    /// </summary>
    public List<string> EntityAssemblyNames { get; set; }

    /// <summary>
    /// 数据库基础实体名称集合
    /// </summary>
    public List<string> BaseEntityNames { get; set; }

    /// <summary>
    /// 基础实体名
    /// </summary>
    public Dictionary<string, string[]> EntityBaseColumn { get; set; }

    /// <summary>
    /// 前端文件根目录
    /// </summary>
    public string FrontRootPath { get; set; }

    /// <summary>
    /// 后端生成到的项目
    /// </summary>
    public List<string> BackendApplicationNamespaces { get; set; }
}