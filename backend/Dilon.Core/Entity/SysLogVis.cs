using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 访问日志表
    /// </summary>
    [Table("sys_log_vis")]
    [Comment("访问日志表")]
    public class SysLogVis : EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 是否执行成功（Y-是，N-否）
        /// </summary>
        [Comment("是否执行成功")]
        public YesOrNot Success { get; set; }

        /// <summary>
        /// 具体消息
        /// </summary>
        [Comment("具体消息")]
        public string Message { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Comment("IP")]
        [MaxLength(20)]
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Comment("地址")]
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [Comment("浏览器")]
        [MaxLength(100)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [Comment("操作系统")]
        [MaxLength(100)]
        public string Os { get; set; }

        /// <summary>
        /// 访问类型
        /// </summary>
        [Comment("访问类型")]
        public LoginType VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        [Comment("访问时间")]
        public DateTimeOffset VisTime { get; set; }

        /// <summary>
        /// 访问人
        /// </summary>
        [Comment("访问人")]
        [MaxLength(20)]
        public string Account { get; set; }
    }
}