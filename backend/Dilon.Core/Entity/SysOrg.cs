using Dilon.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 组织机构表
    /// </summary>
    [Table("sys_org")]
    [Comment("组织机构表")]
    public class SysOrg : DBEntityTenant
    {
        /// <summary>
        /// 父Id
        /// </summary>
        [Comment("父Id")]
        public long Pid { get; set; }

        /// <summary>
        /// 父Ids
        /// </summary>
        [Comment("父Ids")]
        public string Pids { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [Required, MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [Required, MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Comment("联系人")]
        [MaxLength(20)]
        public string Contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        [MaxLength(20)]
        public string Tel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 多对多（用户）
        /// </summary>
        public ICollection<SysUser> SysUsers { get; set; }

        /// <summary>
        /// 多对多中间表（用户数据范围）
        /// </summary>
        public List<SysUserDataScope> SysUserDataScopes { get; set; }

        /// <summary>
        /// 多对多（角色）
        /// </summary>
        public ICollection<SysRole> SysRoles { get; set; }

        /// <summary>
        /// 多对多中间表（角色数据范围）
        /// </summary>
        public List<SysRoleDataScope> SysRoleDataScopes { get; set; }
    }
}
