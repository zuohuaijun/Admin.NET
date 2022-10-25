namespace Admin.NET.Core;

public interface IOnlineUserHub
{
    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ForceOffline(object context);

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ReceiveMessage(object context);

    /// <summary>
    /// 在线用户变动
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnlineUserChange(OnlineUserHubOutput context);

    /// <summary>
    /// 组合消息
    /// </summary>
    /// <param name="notice"></param>
    /// <returns></returns>
    Task AppendNotice(SysNotice notice);
}