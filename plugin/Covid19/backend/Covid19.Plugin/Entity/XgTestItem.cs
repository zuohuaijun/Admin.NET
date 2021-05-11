using Admin.NET.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测项目
    /// </summary>
    [Table("xg_test_item")]
    [Comment("参数配置表")]
    public class XgTestItem : DBEntityTenant
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [Required, MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [MaxLength(30)]
        public string Code { get; set; }

        /// <summary>
        /// 类型（1非定期检测 2定期检测）
        /// </summary>
        [Comment("类型")]
        [MaxLength(2)]
        public string Type { get; set; }

        /// <summary>
        /// 适用层级(全省、全市、全县、本医院)
        /// </summary>
        [Comment("适用层级")]
        [MaxLength(10)]
        public string Area { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(50)]
        public string Remark { get; set; }
    }
}
