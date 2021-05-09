using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 租户表
    /// </summary>
    [Table("sys_tenant")]
    [Comment("租户表")]
    public class SysTenant : DEntityBase<long, MultiTenantDbContextLocator>
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Comment("公司名称")]
        [Required, MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Comment("管理员名称")]
        [Required, MaxLength(20)]
        public string AdminName { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [Comment("主机")]
        [MaxLength(100)]
        public string Host { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Comment("电子邮箱")]
        [MaxLength(20)]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        [Comment("数据库连接")]
        [MaxLength(200)]
        public string Connection { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        [Comment("架构")]
        [MaxLength(50)]
        public string Schema { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }
    }
}