// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Plugin.GoView.Enum;

/// <summary>
/// GoView 项目状态
/// </summary>
[Description("GoView 项目状态")]
public enum GoViewProState
{
    /// <summary>
    /// 未发布
    /// </summary>
    [Description("未发布")]
    UnPublish = -1,

    /// <summary>
    /// 已发布
    /// </summary>
    [Description("已发布")]
    Published = 1,
}