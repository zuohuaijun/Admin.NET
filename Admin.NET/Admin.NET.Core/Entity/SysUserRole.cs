using SqlSugar;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统用户角色表
    /// </summary>
    [SugarTable("sys_user_role", "系统用户角色表")]
    [SqlSugarEntity]
    public class SysUserRole : EntityBaseId
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(ColumnDescription = "用户Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(ColumnDescription = "角色Id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysRole SysRole { get; set; }
    }
}