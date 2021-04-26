using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 在线用户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "OnlineUser", Order = 100)]
    public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<SysUser> _sysUerrep;  // 用户表仓储 

        public SysOnlineUserService(IMemoryCache memoryCache, IRepository<SysUser> sysUerrep)
        {
            _memoryCache = memoryCache;
            _sysUerrep = sysUerrep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysOnlineUser/list")]
        public async Task<dynamic> List()
        {
            var users = await _memoryCache.GetOrCreateAsync("online_users", async _ =>
            {
                return await _sysUerrep.AsQueryable().ToListAsync();
            });
            return users;
        }
    }
}
