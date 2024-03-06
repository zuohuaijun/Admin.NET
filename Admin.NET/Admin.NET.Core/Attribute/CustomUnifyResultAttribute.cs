// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 自定义规范化结果特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class CustomUnifyResultAttribute : Attribute
{
    public string Name { get; set; }

    public CustomUnifyResultAttribute(string name)
    {
        Name = name;
    }
}