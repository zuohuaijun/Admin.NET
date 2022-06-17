using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core.Service;

/// <summary>
/// 消息发送服务
/// </summary>
[ApiDescriptionSettings(Name = "消息发送", Order = 101)]
public class SendMessageService : ISendMessageService, IDynamicApiController, ITransient
{
    private readonly ISysCacheService _sysCacheService;
    private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

    public SendMessageService(ISysCacheService sysCacheService,
        IHubContext<ChatHub, IChatClient> chatHubContext)
    {
        _sysCacheService = sysCacheService;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    [HttpGet("sysMessage/allUser")]
    public async Task SendMessageToAllUser(string title, string message, MessageTypeEnum type)
    {
        await _chatHubContext.Clients.All.ReceiveMessage(new { Title = title, Message = message, Messagetype = type });
    }

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">发送人</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    [HttpGet("sysMessage/otherUser")]
    public async Task SendMessageToOtherUser(string title, string message, MessageTypeEnum type, long userId)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var user = onlineuserlist.Where(x => x.UserId == userId).ToList();
        if (user != null)
        {
            await _chatHubContext.Clients.AllExcept(user[0].ConnectionId).ReceiveMessage(new { Title = title, Message = message, Messagetype = type });
        }
    }

    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">接收人</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    [HttpGet("sysMessage/user")]
    public async Task SendMessageToUser(string title, string message, MessageTypeEnum type, long userId)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var user = onlineuserlist.Where(x => x.UserId == userId).ToList();
        if (user != null)
        {
            foreach (var item in user)
            {
                await _chatHubContext.Clients.Client(item.ConnectionId).ReceiveMessage(new { Title = title, Message = message, Messagetype = type });
            }
        }
    }

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="title">发送标题</param>
    /// <param name="message">发送内容</param>
    /// <param name="userId">接收人列表</param>
    /// <param name="type">消息类型</param>
    /// <returns></returns>
    [HttpGet("sysMessage/users")]
    public async Task SendMessageToUsers(string title, string message, MessageTypeEnum type, List<long> userId)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var userlist = new List<string>();
        foreach (var item in onlineuserlist)
        {
            if (userId.Contains(item.UserId))
                userlist.Add(item.ConnectionId);
        }
        await _chatHubContext.Clients.Clients(userlist).ReceiveMessage(new { Title = title, Message = message, Messagetype = type });
    }
}