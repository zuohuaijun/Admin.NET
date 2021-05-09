using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("sys_menu")]
    [Comment("菜单表")]
    public class SysMenu : DEntityBase
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
        [Required, MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [Required, MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 菜单类型（字典 0目录 1菜单 2按钮）
        /// </summary>
        [Comment("菜单类型")]
        public MenuType Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Comment("图标")]
        [MaxLength(20)]
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [Comment("路由地址")]
        [MaxLength(100)]
        public string Router { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        [Comment("组件地址")]
        [MaxLength(100)]
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        [Comment("权限标识")]
        [MaxLength(100)]
        public string Permission { get; set; }

        /// <summary>
        /// 应用分类（应用编码）
        /// </summary>
        [Comment("应用分类")]
        [MaxLength(50)]
        public string Application { get; set; }

        /// <summary>
        /// 打开方式（字典 0无 1组件 2内链 3外链）
        /// </summary>
        [Comment("打开方式")]
        public MenuOpenType OpenType { get; set; } = MenuOpenType.NONE;

        /// <summary>
        /// 是否可见（Y-是，N-否）
        /// </summary>
        [Comment("是否可见")]
        [MaxLength(5)]
        public string Visible { get; set; } = "Y";

        /// <summary>
        /// 内链地址
        /// </summary>
        [Comment("内链地址")]
        [MaxLength(100)]
        public string Link { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        [Comment("重定向地址")]
        [MaxLength(100)]
        public string Redirect { get; set; }

        /// <summary>
        /// 权重（字典 1系统权重 2业务权重）
        /// </summary>
        [Comment("权重")]
        public MenuWeight Weight { get; set; } = MenuWeight.DEFAULT_WEIGHT;

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; } = 100;

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 多对多（角色）
        /// </summary>
        public ICollection<SysRole> SysRoles { get; set; }

        /// <summary>
        /// 多对多中间表（用户角色）
        /// </summary>
        public List<SysRoleMenu> SysRoleMenus { get; set; }
    }
}