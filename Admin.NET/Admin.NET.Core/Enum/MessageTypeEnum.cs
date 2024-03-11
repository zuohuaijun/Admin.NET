// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 消息类型枚举
/// </summary>
[Description("消息类型枚举")]
public enum MessageTypeEnum
{
    /// <summary>
    /// 普通信息
    /// </summary>
    [Description("消息")]
    Info = 0,

    /// <summary>
    /// 成功提示
    /// </summary>
    [Description("成功")]
    Success = 1,

    /// <summary>
    /// 警告提示
    /// </summary>
    [Description("警告")]
    Warning = 2,

    /// <summary>
    /// 错误提示
    /// </summary>
    [Description("错误")]
    Error = 3
}