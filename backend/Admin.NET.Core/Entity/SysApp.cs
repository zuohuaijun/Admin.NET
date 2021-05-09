using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统应用表
    /// </summary>
    [Table("sys_app")]
    [Comment("系统应用表")]
    public class SysApp : DEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [Required, MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [Required, MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 是否默认激活（Y-是，N-否）,只能有一个系统默认激活
        /// 用户登录后默认展示此系统菜单
        /// </summary>
        [Comment("是否默认激活")]
        [MaxLength(5)]
        public string Active { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }
    }
}