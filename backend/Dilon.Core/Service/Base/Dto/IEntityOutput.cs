using System;

namespace Dilon.Core.Service
{
    public interface IEntityOutput : IEntityId
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        public long? UpdatedUserId { get; set; }
    }
}
