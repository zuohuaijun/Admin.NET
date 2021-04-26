using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 系统应用服务
    /// </summary>
    [ApiDescriptionSettings(Name = "OnlineUser", Order = 100)]
    public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<SysUser> _rep;  // 用户表仓储 

        public SysOnlineUserService(IMemoryCache memoryCache, IRepository<SysUser> rep)
        {
            _memoryCache = memoryCache;
            _rep = rep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysOnlineUser/list")]
        public async Task<dynamic> List()
        {
            var users = await _memoryCache.GetOrCreateAsync("online_users", async entry => { return new List<Core.OnlineUser>(); });
            // TODO: 根据在线用户查找登陆信息, 并返回
            return users;
        }
    }
}
