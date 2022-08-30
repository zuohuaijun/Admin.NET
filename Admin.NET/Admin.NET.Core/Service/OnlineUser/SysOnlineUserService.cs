using Microsoft.AspNetCore.SignalR;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统在线用户服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
{
    private readonly ISysCacheService _sysCacheService;
    private readonly SqlSugarRepository<SysOnlineUser> _sysOnlineUerRep;
    private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

    public SysOnlineUserService(ISysCacheService sysCacheService,
        SqlSugarRepository<SysOnlineUser> sysOnlineUerRep,
        IHubContext<ChatHub, IChatClient> chatHubContext)
    {
        _sysCacheService = sysCacheService;
        _sysOnlineUerRep = sysOnlineUerRep;
        _chatHubContext = chatHubContext;
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
}