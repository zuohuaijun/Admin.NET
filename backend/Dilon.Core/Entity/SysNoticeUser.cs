using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 通知公告用户表
    /// </summary>
    [Table("sys_notice_user")]
    [Comment("通知公告用户表")]
    public class SysNoticeUser : IEntity, IEntityTypeBuilder<SysNoticeUser>
    {
        /// <summary>
        /// 通知公告Id
        /// </summary>
        [Comment("通知公告Id")]
        public long NoticeId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Comment("用户Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        [Comment("阅读时间")]
        public DateTimeOffset ReadTime { get; set; }

        /// <summary>
        /// 状态（字典 0未读 1已读）
        /// </summary>
        [Comment("状态")]
        public NoticeUserStatus ReadStatus { get; set; }

        public void Configure(EntityTypeBuilder<SysNoticeUser> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasKey(c => new { c.NoticeId, c.UserId });
        }
    }
}