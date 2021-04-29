using Furion;
using Furion.DataEncryption;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Hubs
{
    /// <summary>
    /// 聊天集线器
    /// </summary>
    public class ChatHub : Hub
    {
        public IMemoryCache Cache { get; set; } = App.GetService<IMemoryCache>();

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = App.HttpContext.Request.Query["access_token"];
            var claims = JWTEncryption.ReadJwtToken(token)?.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_USERID)?.Value);

            var users = await Cache.GetOrCreateAsync("online_users", async entry =>
            {
                return new List<OnlineUser>();
            });
            if (!string.IsNullOrEmpty(Context.ConnectionId) && userId != 0)
            {
                users.Add(new OnlineUser()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId,
                    LastTime = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (!string.IsNullOrEmpty(Context.ConnectionId))
            {
                var users = await Cache.GetOrCreateAsync("online_users",
                    async entry => { return new List<OnlineUser>(); });

                var user = users.FirstOrDefault(e => e.ConnectionId == Context.ConnectionId);

                if (user != null)
                    users.Remove(user);
            }
        }
    }
}