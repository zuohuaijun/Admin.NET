using System;

namespace Dilon.Core.Service
{
    public abstract class EntityOutput : IEntityOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public virtual long? CreatedUserId { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        public virtual long? UpdatedUserId { get; set; }
    }
}
