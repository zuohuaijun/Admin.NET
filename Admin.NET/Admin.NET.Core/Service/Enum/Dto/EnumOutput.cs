// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 枚举类型输出参数
/// </summary>
public class EnumTypeOutput
{
    /// <summary>
    /// 枚举类型描述
    /// </summary>
    public string TypeDescribe { get; set; }

    /// <summary>
    /// 枚举类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 枚举类型备注
    /// </summary>
    public string TypeRemark { get; set; }
}