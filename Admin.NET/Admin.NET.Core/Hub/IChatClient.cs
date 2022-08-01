namespace Admin.NET.Core;

/// <summary>
/// 聊天客户端接口定义
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// 强制下线
    /// </summary>
    /// <returns></returns>
    Task ForceExist(string str);

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ReceiveMessage(object context);

    /// <summary>
    /// 组合信息
    /// </summary>
    /// <param name="notice"></param>
    /// <returns></returns>
    Task AppendNotice(SysNotice notice);
}