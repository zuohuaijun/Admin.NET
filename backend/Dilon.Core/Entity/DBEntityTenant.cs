using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dilon.Core.Entity
{
   public abstract class DBEntityTenant:IEntity
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Comment("租户id")]
        public virtual long TenantId { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Id主键")]
        public virtual long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        public virtual DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Comment("更新时间")]
        public virtual DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Comment("创建者Id")]
        public virtual long? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [Comment("创建者名称")]
        [MaxLength(20)]
        public virtual string CreatedUserName { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Comment("修改者Id")]
        public virtual long? UpdatedUserId { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        [Comment("修改者名称")]
        [MaxLength(20)]
        public virtual string UpdatedUserName { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [JsonIgnore, FakeDelete(true)]
        [Comment("软删除标记")]
        public virtual bool IsDeleted { get; set; } = false;
    }
}
