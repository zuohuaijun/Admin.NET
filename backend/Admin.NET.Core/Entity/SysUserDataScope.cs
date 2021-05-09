using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 用户数据范围表
    /// </summary>
    [Table("sys_user_data_scope")]
    [Comment("用户数据范围表")]
    public class SysUserDataScope : IEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Comment("用户Id")]
        public long SysUserId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysUser SysUser { get; set; }

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