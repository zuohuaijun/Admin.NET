using Furion.Snowflake;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 组织机构表
    /// </summary>
    [Table("sys_org")]
    public class SysOrg : DEntityBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 父Ids
        /// </summary>
        public string Pids { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
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
