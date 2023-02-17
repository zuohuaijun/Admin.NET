using FluentEmail.Core;
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
    private readonly IFluentEmail _fluentEmail;
    private readonly IHubContext<OnlineUserHub, IOnlineUserHub> _chatHubContext;

    public SysMessageService(SysCacheService sysCacheService,
        IOptions<EmailOptions> emailOptions,
        IFluentEmail fluentEmail,
        IHubContext<OnlineUserHub, IOnlineUserHub> chatHubContext)
    {
        _sysCacheService = sysCacheService;
        _emailOptions = emailOptions.Value;
        _fluentEmail = fluentEmail;
        _chatHubContext = chatHubContext;
    }

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SendAllUser")]
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
    [ApiDescriptionSettings(Name = "SendOtherUser")]
    [DisplayName("发送消息给除了发送人的其他人")]
    public async Task SendOtherUser(MessageInput input)
    {
        var onlineuserlist = _sysCacheService.Get<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var user = onlineuserlist.Where(x => x.UserId == input.UserId).ToList();
        if (user != null)
        {
            await _chatHubContext.Clients.AllExcept(user[0].ConnectionId).ReceiveMessage(input);
        }
    }

    /// <summary>
    /// 发送消息给某个人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SendUser")]
    [DisplayName("发送消息给某个人")]
    public async Task SendUser(MessageInput input)
    {
        var onlineuserlist = _sysCacheService.Get<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var user = onlineuserlist.Where(x => x.UserId == input.UserId).ToList();
        if (user == null) return;

        foreach (var item in user)
        {
            await _chatHubContext.Clients.Client(item.ConnectionId).ReceiveMessage(input);
        }
    }

    /// <summary>
    /// 发送消息给某些人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SendUsers")]
    [DisplayName("发送消息给某些人")]
    public async Task SendUsers(MessageInput input)
    {
        var onlineuserlist = _sysCacheService.Get<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

        var userlist = new List<string>();
        foreach (var item in onlineuserlist)
        {
            if (input.UserIds.Contains(item.UserId))
                userlist.Add(item.ConnectionId);
        }
        await _chatHubContext.Clients.Clients(userlist).ReceiveMessage(input);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SendEmail")]
    [DisplayName("发送邮件")]
    public async Task SendEmail([Required] string message)
    {
        await _fluentEmail.To(_emailOptions.DefaultToEmail).Body(message).SendAsync();
    }
}