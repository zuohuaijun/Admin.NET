using Microsoft.AspNetCore.SignalR;
using NETCore.MailKit.Core;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统消息发送服务
/// </summary>
[ApiDescriptionSettings(Order = 101)]
public class SysMessageService : ISysMessageService, IDynamicApiController, ITransient
{
    private readonly ISysCacheService _sysCacheService;
    private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

    private readonly EmailOptions _emailOptions;
    private readonly IEmailService _emailService;

    public SysMessageService(ISysCacheService sysCacheService,
        IHubContext<ChatHub, IChatClient> chatHubContext,
        IOptions<EmailOptions> emailOptions,
        IEmailService emailService)
    {
        _sysCacheService = sysCacheService;
        _chatHubContext = chatHubContext;
        _emailOptions = emailOptions.Value;
        _emailService = emailService;
    }

    /// <summary>
    /// 发送消息给所有人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("sysMessage/allUser")]
    public async Task SendMessageToAllUser(MessageInput input)
    {
        await _chatHubContext.Clients.All.ReceiveMessage(input);
    }

    /// <summary>
    /// 发送消息给除了发送人的其他人
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("sysMessage/otherUser")]
    public async Task SendMessageToOtherUser(MessageInput input)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

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
    [HttpPost("sysMessage/user")]
    public async Task SendMessageToUser(MessageInput input)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

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
    [HttpPost("sysMessage/users")]
    public async Task SendMessageToUsers(MessageInput input)
    {
        var onlineuserlist = await _sysCacheService.GetAsync<List<SysOnlineUser>>(CacheConst.KeyOnlineUser);

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
    [HttpPost("/email/send")]
    public async Task SendEmail(string message)
    {
        //// 设置发送人邮件地址和名称
        //var sendInfo = new SenderInfo
        //{
        //    SenderEmail = _emailOptions.SenderEmail,
        //    SenderName = _emailOptions.SenderName,
        //};
        await _emailService.SendAsync(_emailOptions.ToEmail, _emailOptions.SenderName, message);
    }
}