using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 角色数据范围表
    /// </summary>
    [Table("sys_role_data_scope")]
    public class SysRoleDataScope : IEntity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long SysRoleId { get; set; }

        /// <summary>
        /// 一对一引用（系统角色）
        /// </summary>
        public SysRole SysRole { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long SysOrgId { get; set; }

        /// <summary>
        /// 一对一引用（系统机构）
        /// </summary>
        public SysOrg SysOrg { get; set; }
    }
}
