using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Admin.NET.Core
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManager : IUserManager, IScoped
    {
        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly IRepository<SysEmp> _sysEmpRep;   // 员工表
        private readonly IHttpContextAccessor _httpContextAccessor;

        public long UserId
        {
            get => long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value);
        }

        public string Account
        {
            get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
        }

        public string Name
        {
            get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value;
        }

        public bool SuperAdmin
        {
            get => _httpContextAccessor.HttpContext.User.FindFirst(ClaimConst.CLAINM_SUPERADMIN)?.Value == ((int)AdminType.SuperAdmin).ToString();
        }

        public SysUser User
        {
            get => _sysUserRep.FirstOrDefault(u => u.Id == UserId, false);
        }

        public UserManager(IRepository<SysUser> sysUserRep,
                           IRepository<SysEmp> sysEmpRep,
                           IHttpContextAccessor httpContextAccessor)
        {
            _sysUserRep = sysUserRep;
            _sysEmpRep = sysEmpRep;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public async Task<SysUser> CheckUserAsync(long userId, bool tracking = true)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == userId, tracking);
            return user ?? throw Oops.Oh(ErrorCode.D1002);
        }

        /// <summary>
        /// 获取用户员工信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SysEmp> GetUserEmpInfo(long userId)
        {
            var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == userId, false);
            return emp ?? throw Oops.Oh(ErrorCode.D1002);
        }
    }
}