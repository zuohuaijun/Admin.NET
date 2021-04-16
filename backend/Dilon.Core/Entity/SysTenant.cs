using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 租户表
    /// </summary>
    [Table("sys_tenant")]
    [Comment("租户表")]
    public class SysTenant : DEntityBase, IEntity<MultiTenantDbContextLocator>
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Comment("公司名称")]
        public string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Comment("管理员名称")]
        public string AdminName { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [Comment("主机")]
        public string Host { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Comment("电子邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        [Comment("数据库连接")]
        public string Connection { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        [Comment("架构")]
        public string Schema { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        public string Remark { get; set; }
    }
}
