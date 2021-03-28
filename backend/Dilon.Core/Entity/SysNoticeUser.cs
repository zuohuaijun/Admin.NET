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
    public class SysNoticeUser : IEntity, IEntityTypeBuilder<SysNoticeUser>
    {
        /// <summary>
        /// 通知公告id
        /// </summary>
        public long NoticeId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTimeOffset ReadTime { get; set; }

        /// <summary>
        /// 状态（字典 0未读 1已读）
        /// </summary>
        public int ReadStatus { get; set; }

        public void Configure(EntityTypeBuilder<SysNoticeUser> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasNoKey();
        }
    }
}
