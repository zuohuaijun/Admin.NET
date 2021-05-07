using Dilon.Core.Service;
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
        private readonly ISysCacheService _cache;

        public ChatHub(ISysCacheService cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var claims = JWTEncryption.ReadJwtToken(token)?.Claims;
            var userId = claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_USERID)?.Value;
            var onlineUsers = await _cache.GetAsync<List<OnlineUser>>(CommonConst.CACHE_KEY_ONLINE_USER);
            if (onlineUsers == null)
                onlineUsers = new List<OnlineUser>();
            onlineUsers.Add(new OnlineUser()
            {
                ConnectionId = Context.ConnectionId,
                UserId = long.Parse(userId),
                LastTime = DateTime.Now
            });
            await _cache.SetAsync(CommonConst.CACHE_KEY_ONLINE_USER, onlineUsers);
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
                var onlineUsers = await _cache.GetAsync<List<OnlineUser>>(CommonConst.CACHE_KEY_ONLINE_USER);
                if (onlineUsers == null) return;

                for (int i = 0; i < onlineUsers.Count; i++)
                {
                    if (onlineUsers[i].ConnectionId == Context.ConnectionId)
                    {
                        onlineUsers.RemoveAt(i);
                        return;
                    }
                }

                await _cache.SetAsync(CommonConst.CACHE_KEY_ONLINE_USER, onlineUsers);
            }
        }
    }
}