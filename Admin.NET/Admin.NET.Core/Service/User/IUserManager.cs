namespace Admin.NET.Core;

public interface IUserManager
{
    long UserId { get; }

    string UserName { get; }

    string RealName { get; }

    bool SuperAdmin { get; }

    string OpenId { get; }

    SysUser User { get; }

    Task<SysUser> CheckUserAsync(long userId);
}