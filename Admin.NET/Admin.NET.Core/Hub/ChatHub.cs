using Furion.InstantMessaging;
using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core;

/// <summary>
/// 聊天集线器
/// </summary>
[MapHub("/hubs/chathub")]
public class ChatHub : Hub<IChatClient>
{
    private readonly ISysCacheService _cache;
    private readonly ISysMessageService _sysMessageService;
    private readonly SqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;
    private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

    public ChatHub(ISysCacheService cache,
        ISysMessageService sysMessageService,
        SqlSugarRepository<SysOnlineUser> sysOnlineUerRep,
        IHubContext<ChatHub, IChatClient> chatHubContext)
    {
        _cache = cache;
        _sysMessageService = sysMessageService;
        _sysOnlineUerRep = sysOnlineUerRep;
        _chatHubContext = chatHubContext;
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
        var user = new SysOnlineUser
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
        };
        await _sysOnlineUerRep.AsInsertable(user).ExecuteCommandAsync();
        //加入分组  以租户ID分组 方便后续通知
        await _chatHubContext.Groups.AddToGroupAsync(Context.ConnectionId, $"{ChatHubPrefix.GROUP_ONLINE}{tenantId}");

        var list = await _sysOnlineUerRep.AsQueryable().Filter("", true).Where(x => x.TenantId == user.TenantId).ToListAsync();
        await _chatHubContext.Clients.Groups($"{ChatHubPrefix.GROUP_ONLINE}{user.TenantId}").OnlineUserChanged(new OnlineUserChangedDto
        {
            Name = user.Name,
            Offline = false,
            List = list
        });

        //onlineUsers.Add();
        //await _cache.SetAsync($"{CacheConst.KeyOnlineUser}{ Context.ConnectionId}", user);

        //await _sendMessageService.SendMessageToUserByConnectionId("asdasd但凡生得分", "下线吧", MessageTypeEnum.Offline, Context.ConnectionId);
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
            var user = await _sysOnlineUerRep.AsQueryable().Filter("", true).FirstAsync(x => x.ConnectionId == Context.ConnectionId);
            if (user == null) return;

            await _sysOnlineUerRep.DeleteAsync(x => x.Id == user.Id);
            //通知当前组用户变动
            var list = await _sysOnlineUerRep.AsQueryable().Filter("", true).Where(x => x.TenantId == user.TenantId).ToListAsync();
            await _chatHubContext.Clients.Groups($"{ChatHubPrefix.GROUP_ONLINE}{user.TenantId}").OnlineUserChanged(new OnlineUserChangedDto
            {
                Name = user.Name,
                Offline = true,
                List = list
            });

            //var onlineUsers = await _cache.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);
            //if (onlineUsers == null) return;

            //onlineUsers.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            //await _cache.RemoveAsync($"{CacheConst.KeyOnlineUser}{ Context.ConnectionId}");
        }
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task ForceExistUser(ForceExistUserRequest request)
    {
        await _chatHubContext.Clients.Client(request.ConnectionId).ForceExist("强制下线");
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给某个人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessage(MessageInput message)
    {
        await _sysMessageService.SendMessageToUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送信息给所有人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoAll(MessageInput message)
    {
        await _sysMessageService.SendMessageToAllUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给除了发送人的其他人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoOther(MessageInput message)
    {
        // _message.userId为发送人ID
        await _sysMessageService.SendMessageToOtherUser(message);
    }

    /// <summary>
    /// 前端调用发送方法（发送消息给某些人）
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ClientsSendMessagetoUsers(MessageInput message)
    {
        await _sysMessageService.SendMessageToUsers(message);
    }
}