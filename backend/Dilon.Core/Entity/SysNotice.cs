using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 通知公告表
    /// </summary>
    [Table("sys_notice")]
    [Comment("通知公告表")]
    public class SysNotice : DEntityBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Comment("标题")]
        [Required, MaxLength(20)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Comment("内容")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 类型（字典 1通知 2公告）
        /// </summary>
        [Comment("类型")]
        public int Type { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        [Comment("发布人Id")]
        public long PublicUserId { get; set; }

        /// <summary>
        /// 发布人姓名
        /// </summary>
        [Comment("发布人姓名")]
        [MaxLength(20)]
        public string PublicUserName { get; set; }

        /// <summary>
        /// 发布机构Id
        /// </summary>
        [Comment("发布机构Id")]
        public long PublicOrgId { get; set; }

        /// <summary>
        /// 发布机构名称
        /// </summary>
        [Comment("发布机构名称")]
        [MaxLength(50)]
        public string PublicOrgName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Comment("发布时间")]
        public DateTimeOffset PublicTime { get; set; }

        /// <summary>
        /// 撤回时间
        /// </summary>
        [Comment("撤回时间")]
        public DateTimeOffset CancelTime { get; set; }

        /// <summary>
        /// 状态（字典 0草稿 1发布 2撤回 3删除）
        /// </summary>
        [Comment("状态")]
        public NoticeStatus Status { get; set; }
    }
}
