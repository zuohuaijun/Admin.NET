// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

public class MessageInput
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户ID列表
    /// </summary>
    public List<long> UserIds { get; set; }

    /// <summary>
    /// 消息标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public MessageTypeEnum MessageType { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Message { get; set; }
}