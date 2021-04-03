using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dilon.Core
{
    /// <summary>
    /// 自定义实体基类
    /// </summary>
    public abstract class DEntityBase : IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        /// 创建者名称
        /// </summary>
        public virtual string CreatedUserName { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        public virtual long? UpdatedUserId { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        public virtual string UpdatedUserName { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [JsonIgnore, FakeDelete(true)]
        public virtual bool IsDeleted { get; set; } = false;
    }
}
