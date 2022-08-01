namespace Admin.NET.Core.Service;

public interface ISysOnlineUserService
{
    Task<dynamic> List(BasePageInput input);

    Task ForceExist(SysOnlineUser user);

    Task PushNotice(SysNotice notice, List<long> userIds);
}