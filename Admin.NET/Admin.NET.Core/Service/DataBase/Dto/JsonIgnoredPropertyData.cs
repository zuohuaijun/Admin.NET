// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 保存标注了JsonIgnore的Property的值信息
/// </summary>
public class JsonIgnoredPropertyData
{
    /// <summary>
    /// 记录索引
    /// </summary>
    public int RecordIndex { get; set; }

    /// <summary>
    /// 属性名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 属性值描述
    /// </summary>
    public string Value { get; set; }
}