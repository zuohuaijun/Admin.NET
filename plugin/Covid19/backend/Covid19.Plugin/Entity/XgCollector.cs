using Admin.NET.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Covid19.Plugin
{
    /// <summary>
    /// 样本采集人员
    /// </summary>
    [Table("xg_collector")]
    [Comment("样本采集人员表")]
    public class XgCollector : DBEntityTenant
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Comment("姓名")]
        [Required, MaxLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Comment("证件号码")]
        [Required, MaxLength(30)]
        public string IdNumber { get; set; }

        /// <summary>
        /// 证件类型（1身份证 2护照 3其他）
        /// </summary>
        [Comment("证件类型")]
        [Required, MaxLength(5)]
        public string IdType { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Comment("手机")]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        [Comment("性别")]
        public Gender Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Comment("生日")]
        public DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        [Comment("住址")]
        [MaxLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        [Comment("采集日期")]
        public DateTimeOffset CollectionTime { get; set; }

        /// <summary>
        /// 采集编号
        /// </summary>
        [Comment("采集编号")]
        [MaxLength(50)]
        public string Number { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        [Comment("站点Id")]
        [MaxLength(50)]
        public string SiteId { get; set; }

        /// <summary>
        /// 社会信用码
        /// </summary>
        [Comment("社会信用码")]
        [MaxLength(50)]
        public string SocialCode { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [Comment("工作单位")]
        [MaxLength(50)]
        public string WorkUnit { get; set; }

        /// <summary>
        /// 工作岗位
        /// </summary>
        [Comment("工作岗位")]
        [MaxLength(20)]
        public string Job { get; set; }

        /// <summary>
        /// 疫情重点地区
        /// </summary>
        [Comment("疫情重点地区")]
        [MaxLength(50)]
        public string EpidemicArea { get; set; }

        /// <summary>
        /// 人员类别编码
        /// </summary>
        [Comment("人员类别编码")]
        [MaxLength(20)]
        public string TypeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("人员类别编码")]
        [MaxLength(50)]
        public string Remark { get; set; }
    }
}