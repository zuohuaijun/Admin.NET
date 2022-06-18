using Furion.InstantMessaging;
using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core;

/// <summary>
/// 聊天集线器
/// </summary>
[MapHub("/hub/chathub")]
public class ChatHub : Hub<IChatClient>
{
    private readonly ISysCacheService _cache;
    private readonly ISendMessageService _sendMessageService;

    public ChatHub(ISysCacheService cache,
        ISendMessageService sendMessageService)
    {
        _cache = cache;
        _sendMessageService = sendMessageService;
    }

    /// <summary>
    /// 连接
    /// </summary>
    /// <returns></returns>
    public override async Task OnConnectedAsync()
    {
        var token = Context.GetHttpContext().Request.Query["access_token"];
        var claims = JWTEncryption.ReadJwtToken(token)?.Claims;

        var client = Parser.GetDefault().Parse(Context.GetHttpContext().Request.Headers["User-Agent"]);
        var loginBrowser = client.UA.Family + client.UA.Major;
        var loginOs = client.OS.Family + client.OS.Major;

        var userId = claims.FirstOrDefault(e => e.Type == ClaimConst.UserId)?.Value;
        var account = claims.FirstOrDefault(e => e.Type == ClaimConst.UserName)?.Value;
        var name = claims.FirstOrDefault(e => e.Type == ClaimConst.RealName)?.Value;
        var tenantId = claims.FirstOrDefault(e => e.Type == ClaimConst.TenantId)?.Value;
        var onlineUsers = await _cache.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser) ?? new List<SysOnlineUser>();
        onlineUsers.Add(new SysOnlineUser
        {
            ConnectionId = Context.ConnectionId,
            UserId = long.Parse(userId),
            LastTime = DateTime.Now,
            LastLoginIp = App.HttpContext.GetRemoteIpAddressToIPv4(),
            LastLoginBrowser = loginBrowser,
            LastLoginOs = loginOs,
            Account = account,
            Name = name,
            TenantId = Convert.ToInt64(tenantId),
        });
        await _cache.SetAsync(CacheConst.KeyOnlineUser, onlineUsers);
    }

    /// <summary>
    /// 断开
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (!string.IsNullOrEmpty(Context.ConnectionId))
        {
            var onlineUsers = await _cache.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);
            if (onlineUsers == null) return;

            onlineUsers.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            await _cache.SetAsync(CacheConst.KeyOnlineUser, onlineUsers);
        }
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给某个人）
    /// </summary>
    /// <param name="_message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessage(MessageInput _message)
    {
        await _sendMessageService.SendMessageToUser(_message.Title, _message.Message, _message.MessageType, _message.UserId);
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给所有人）
    /// </summary>
    /// <param name="_message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoAll(MessageInput _message)
    {
        await _sendMessageService.SendMessageToAllUser(_message.Title, _message.Message, _message.MessageType);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给除了发送人的其他人）
    /// </summary>
    /// <param name="_message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoOther(MessageInput _message)
    {
        await _sendMessageService.SendMessageToOtherUser(_message.Title, _message.Message, _message.MessageType, _message.UserId);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给某些人）
    /// </summary>
    /// <param name="_message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoUsers(MessageInput _message)
    {
        await _sendMessageService.SendMessageToUsers(_message.Title, _message.Message, _message.MessageType, _message.UserIds);
    }
}