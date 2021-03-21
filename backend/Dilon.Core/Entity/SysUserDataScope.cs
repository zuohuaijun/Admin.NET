using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 用户数据范围表
    /// </summary>
    [Table("sys_user_data_scope")]
    public class SysUserDataScope : IEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long SysUserId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysUser SysUser { get; set; }

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
