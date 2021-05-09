using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("sys_role_menu")]
    [Comment("角色菜单表")]
    public class SysRoleMenu : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Comment("角色Id")]
        public long SysRoleId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysRole SysRole { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Comment("菜单Id")]
        public long SysMenuId { get; set; }

        /// <summary>
        /// 一对一引用（系统菜单）
        /// </summary>
        public SysMenu SysMenu { get; set; }
    }
}