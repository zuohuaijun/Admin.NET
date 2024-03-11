// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 通知公告用户状态枚举
/// </summary>
[Description("通知公告用户状态枚举")]
public enum NoticeUserStatusEnum
{
    /// <summary>
    /// 未读
    /// </summary>
    [Description("未读")]
    UNREAD = 0,

    /// <summary>
    /// 已读
    /// </summary>
    [Description("已读")]
    READ = 1
}