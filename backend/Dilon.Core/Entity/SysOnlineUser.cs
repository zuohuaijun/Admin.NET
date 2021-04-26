using System;

namespace Dilon.Core
{
    public class OnlineUser
    {
        /// <summary>
        /// 连接Id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// userId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 最后连接时间
        /// </summary>
        public DateTime LastTime { get; set; }
    }
}