using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 租户表
    /// </summary>
    [Table("sys_tenant")]
    public class SysTenant : DEntityBase, IEntity<MultiTenantDbContextLocator>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
