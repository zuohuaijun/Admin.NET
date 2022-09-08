using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统在线用户服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;
    private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;
    private readonly SysConfigService _sysConfigService;

    public SysOnlineUserService(SqlSugarRepository<SysOnlineUser> sysOnlineUerRep,
        IHubContext<ChatHub, IChatClient> chatHubContext,
        SysConfigService sysConfigService)
    {
        _sysOnlineUerRep = sysOnlineUerRep;
        _chatHubContext = chatHubContext;
        _sysConfigService = sysConfigService;
    }

    /// <summary>
    /// 获取在线用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysOnlineUser/page")]
    public async Task<dynamic> List([FromQuery] BasePageInput input)
    {
        return await _sysOnlineUerRep.AsQueryable().ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("/sysOnlineUser/forceExist")]
    [NonValidation]
    public async Task ForceExist(SysOnlineUser user)
    {
        await _chatHubContext.Clients.Client(user.ConnectionId).ForceExist("00000000");
        await _sysOnlineUerRep.DeleteAsync(user);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="notice"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    [NonAction]
    public async Task PushNotice(SysNotice notice, List<long> userIds)
    {
        var userList = await _sysOnlineUerRep.GetListAsync(m => userIds.Contains(m.UserId));
        if (userList.Any())
        {
            foreach (var item in userList)
            {
                await _chatHubContext.Clients.Client(item.ConnectionId).AppendNotice(notice);
            }
        }
    }

    /// <summary>
    /// 单用户登录
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task SignleLogin(long userId)
    {
        if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSingleLogin))
        {
            var onlineUsers = await _sysOnlineUerRep.GetListAsync();
            if (onlineUsers == null) return;

            var loginUser = onlineUsers.FirstOrDefault(u => u.UserId == userId);
            if (loginUser == null) return;

            await ForceExist(loginUser);
        }
    }
}