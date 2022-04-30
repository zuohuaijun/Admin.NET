using SqlSugar;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统角色菜单表
    /// </summary>
    [SugarTable("sys_role_menu", "系统角色菜单表")]
    [SqlSugarEntity]
    public class SysRoleMenu : EntityBaseId
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(ColumnDescription = "角色Id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单Id")]
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysMenu SysMenu { get; set; }
    }
}