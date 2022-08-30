namespace Admin.NET.Core.Service;

public interface ISysMessageService
{
    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendMessageToAllUser(MessageInput input);

    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendMessageToUser(MessageInput input);

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendMessageToUsers(MessageInput input);

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendMessageToOtherUser(MessageInput input);

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendEmail(string message);
}