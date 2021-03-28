using Furion.Snowflake;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("sys_menu")]
    public class SysMenu : DEntityBase
    {
        public SysMenu()
        {
            Id = IDGenerator.NextId();
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Status = (int)CommonStatus.ENABLE;
         }

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
        [Required, MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 菜单类型（字典 0目录 1菜单 2按钮）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Router { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 应用分类（应用编码）
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 打开方式（字典 0无 1组件 2内链 3外链）
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 是否可见（Y-是，N-否）
        /// </summary>
        public string Visible { get; set; }

        /// <summary>
        /// 内链地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 权重（字典 1系统权重 2业务权重）
        /// </summary>
        public int Weight { get; set; }

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
        public int Status { get; set; }

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
