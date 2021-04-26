using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dilon.Core;
using Furion;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Dilon.Web.Core.Hubs
{
    public class ChatHub : Hub
    {
        public IMemoryCache Cache { get; set; } = App.GetService<IMemoryCache>();

        public override async Task OnConnectedAsync()
        {
            // 获得 token
            string token = App.HttpContext.Request.Query["access_token"];
            var headers = App.HttpContext.Request.Headers;
            var clientInfo = headers.ContainsKey("User-Agent")
                ? UAParser.Parser.GetDefault().Parse(headers["User-Agent"])
                : null;
            // 解析 token
            var claims = JWTEncryption.ReadJwtToken(token)?.Claims;
            var userId = Convert.ToInt64(claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_USERID)?.Value);


            var users = await Cache.GetOrCreateAsync("online_users", async entry => { return new List<OnlineUser>(); });

            if (!string.IsNullOrEmpty(Context.ConnectionId) && userId != 0)
            {
                users.Add(new OnlineUser()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId,
                    LastConnectionTime = DateTime.Now,
                    Ip = App.HttpContext.GetRemoteIpAddressToIPv4(),
                    Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                });
            }
        }

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

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToCurrent(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }
}