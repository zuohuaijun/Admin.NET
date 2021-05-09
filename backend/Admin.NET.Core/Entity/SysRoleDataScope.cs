using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 角色数据范围表
    /// </summary>
    [Table("sys_role_data_scope")]
    [Comment("角色数据范围表")]
    public class SysRoleDataScope : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Comment("角色Id")]
        public long SysRoleId { get; set; }

        /// <summary>
        /// 一对一引用（系统角色）
        /// </summary>
        public SysRole SysRole { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        [Comment("机构Id")]
        public long SysOrgId { get; set; }

        /// <summary>
        /// 一对一引用（系统机构）
        /// </summary>
        public SysOrg SysOrg { get; set; }
    }
}