using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("sys_user_role")]
    public class SysUserRole : IEntity
    {
        /// <summary>
        /// 系统用户Id
        /// </summary>
        public long SysUserId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysUser SysUser { get; set; }

        /// <summary>
        /// 系统角色Id
        /// </summary>
        public long SysRoleId { get; set; }

        /// <summary>
        /// 一对一引用（系统角色）
        /// </summary>
        public SysRole SysRole { get; set; }
    }
}
