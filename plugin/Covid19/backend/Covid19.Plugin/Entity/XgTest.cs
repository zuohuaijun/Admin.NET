using Admin.NET.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测
    /// </summary>
    [Table("xg_test")]
    [Comment("核酸检测")]
    public class XgTest : DBEntityTenant
    {
        /// <summary>
        /// 采集人员Id
        /// </summary>
        [Comment("采集人员Id")]
        public long CollectorId { get; set; }

        /// <summary>
        /// 新冠ORFlab
        /// </summary>
        [Comment("新冠ORFlab")]
        public int XgOrflab { get; set; }

        /// <summary>
        /// 新冠N
        /// </summary>
        [Comment("新冠N")]
        public int XgN { get; set; }

        /// <summary>
        /// 血清IgG
        /// </summary>
        [Comment("血清IgG")]
        [MaxLength(10)]
        public string IgG { get; set; }

        /// <summary>
        /// 血清IgM
        /// </summary>
        [Comment("血清IgM")]
        [MaxLength(10)]
        public string IgM { get; set; }

        /// <summary>
        /// 检验医生
        /// </summary>
        [Comment("检验医生")]
        [MaxLength(10)]
        public string TestDoctor { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        [Comment("检验时间")]
        public DateTimeOffset? TestTime { get; set; }

        /// <summary>
        /// 审核医生
        /// </summary>
        [Comment("检验时间")]
        [MaxLength(10)]
        public string AuditDoctor { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Comment("审核时间")]
        public DateTimeOffset? AuditTime { get; set; }

        /// <summary>
        /// 一对一引用（采集人员）
        /// </summary>
        public XgCollector Collector { get; set; }
    }
}
