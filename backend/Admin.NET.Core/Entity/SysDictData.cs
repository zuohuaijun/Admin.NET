using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 字典值表
    /// </summary>
    [Table("sys_dict_data")]
    [Comment("字典值表")]
    public class SysDictData : DEntityBase
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Comment("字典类型Id")]
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Comment("值")]
        [MaxLength(100)]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 所属类型
        /// </summary>
        public SysDictType SysDictType { get; set; }
    }
}