using System;

namespace Dilon.Core
{
    public class OnlineUser
    {
        public string ConnectionId { get; set; }

        public DateTime LastConnectionTime { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }

        public long UserId { get; set; }
    }
}