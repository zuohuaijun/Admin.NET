using System.Threading.Tasks;

namespace Admin.NET.Core
{
    public interface IUserManager
    {
        string Account { get; }
        string Name { get; }
        bool SuperAdmin { get; }
        SysUser User { get; }
        long UserId { get; }

        Task<SysUser> CheckUserAsync(long userId, bool tracking = true);

        Task<SysEmp> GetUserEmpInfo(long userId);
    }
}