// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 通知公告状类型枚举
/// </summary>
[Description("通知公告状类型枚举")]
public enum NoticeTypeEnum
{
    /// <summary>
    /// 通知
    /// </summary>
    [Description("通知")]
    NOTICE = 1,

    /// <summary>
    /// 公告
    /// </summary>
    [Description("公告")]
    ANNOUNCEMENT = 2,
}