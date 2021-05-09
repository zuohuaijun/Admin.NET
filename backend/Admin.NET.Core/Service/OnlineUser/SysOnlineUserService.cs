using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 在线用户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "OnlineUser", Order = 100)]
    public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
    {
        private readonly ISysCacheService _sysCacheService;
        private readonly IRepository<SysUser> _sysUerrep;  // 用户表仓储

        public SysOnlineUserService(ISysCacheService sysCacheService, IRepository<SysUser> sysUerrep)
        {
            _sysCacheService = sysCacheService;
            _sysUerrep = sysUerrep;
        }

        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysOnlineUser/list")]
        public async Task<dynamic> List()
        {
            var onlineUsers = await _sysCacheService.GetAsync<List<OnlineUser>>(CommonConst.CACHE_KEY_ONLINE_USER);
            var ids = onlineUsers.Select(u => u.UserId);
            return await _sysUerrep.Where(u => ids.Contains(u.Id)).ToListAsync();
        }
    }
}