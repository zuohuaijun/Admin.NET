// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统消息发送服务
/// </summary>
[ApiDescriptionSettings(Order = 370)]
public class SysMessageService : IDynamicApiController, ITransient
{
    private readonly SysCacheService _sysCacheService;
    private readonly EmailOptions _emailOptions;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _chatHubContext;

    public SysMessageService(SysCacheService sysCacheService,
        IOptions<EmailOptions> emailOptions,
        IHubContext<OnlineUserHub, IOnlineUserHub> chatHubContext)
    {
        _sysCacheService = sysCacheService;
        _emailOptions = emailOptions.Value;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("发送消息给所有人")]
    public async Task SendAllUser(MessageInput input)
    {
        await _chatHubContext.Clients.All.ReceiveMessage(input);
    }

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("发送消息给除了发送人的其他人")]
    public async Task SendOtherUser(MessageInput input)
    {
        var user = _sysCacheService.Get<SysOnlineUser>(CacheConst.KeyUserOnline + input.UserId);
        if (user != null)
        {
            await _chatHubContext.Clients.AllExcept(user.ConnectionId).ReceiveMessage(input);
        }
    }

    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("发送消息给某个人")]
    public async Task SendUser(MessageInput input)
    {
        var user = _sysCacheService.Get<SysOnlineUser>(CacheConst.KeyUserOnline + input.UserId);
        if (user == null) return;
        await _chatHubContext.Clients.Client(user.ConnectionId).ReceiveMessage(input);
        // 可以直接通过用户Id发消息
        //await _chatHubContext.Clients.User(user.UserId.ToString()).ReceiveMessage(input);
    }

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("发送消息给某些人")]
    public async Task SendUsers(MessageInput input)
    {
        var userlist = new List<string>();
        foreach (var userid in input.UserIds)
        {
            var user = _sysCacheService.Get<SysOnlineUser>(CacheConst.KeyUserOnline + userid);
            if (user != null) userlist.Add(user.ConnectionId);
        }
        await _chatHubContext.Clients.Clients(userlist).ReceiveMessage(input);
    }
}