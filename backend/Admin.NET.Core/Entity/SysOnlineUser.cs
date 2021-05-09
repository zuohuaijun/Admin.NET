using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 在线用户表
    /// </summary>
    [Table("sys_online_user")]
    [Comment("在线用户表")]
    public class OnlineUser : IEntity, IEntityTypeBuilder<OnlineUser>
    {
        /// <summary>
        /// 连接Id
        /// </summary>
        [Comment("连接Id")]
        public string ConnectionId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Comment("用户Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 最后连接时间
        /// </summary>
        [Comment("最近时间")]
        public DateTime LastTime { get; set; }

        public void Configure(EntityTypeBuilder<OnlineUser> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasKey(c => new { c.UserId });
        }
    }
}