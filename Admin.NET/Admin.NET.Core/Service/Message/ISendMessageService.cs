namespace Admin.NET.Core.Service;

public interface ISendMessageService
{
    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">接收人</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    Task SendMessageToUser(string title, string message, MessageTypeEnum type, long userId);

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">接收人列表</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    Task SendMessageToUsers(string title, string message, MessageTypeEnum type, List<long> userId);

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    Task SendMessageToAllUser(string title, string message, MessageTypeEnum type);

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">发送人</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    Task SendMessageToOtherUser(string title, string message, MessageTypeEnum type, long userId);
}