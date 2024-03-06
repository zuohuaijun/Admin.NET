// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 接口/动态API输出
/// </summary>
public class ApiOutput
{
    /// <summary>
    /// 组名称
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 路由名称
    /// </summary>
    public string RouteName { get; set; }
}