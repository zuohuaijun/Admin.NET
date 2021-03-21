using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("sys_role_menu")]
    public class SysRoleMenu : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long SysRoleId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysRole SysRole { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long SysMenuId { get; set; }

        /// <summary>
        /// 一对一引用（系统菜单）
        /// </summary>
        public SysMenu SysMenu { get; set; }
    }
}
