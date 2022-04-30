using SqlSugar;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统用户附属机构职位表
    /// </summary>
    [SugarTable("sys_user_ext_org_pos", "系统用户附属机构职位表")]
    [SqlSugarEntity]
    public class SysUserExtOrgPos : EntityBaseId
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(ColumnDescription = "用户Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysUser SysUser { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        [SugarColumn(ColumnDescription = "机构Id")]
        public long OrgId { get; set; }

        /// <summary>
        /// 机构
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysOrg SysOrg { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        [SugarColumn(ColumnDescription = "职位Id")]
        public long PosId { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysPos SysPos { get; set; }
    }
}