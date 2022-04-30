using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统菜单表
    /// </summary>
    [SugarTable("sys_menu", "系统菜单表")]
    [SqlSugarEntity]
    public class SysMenu : EntityBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        [SugarColumn(ColumnDescription = "父Id")]
        public long Pid { get; set; }

        /// <summary>
        /// 菜单类型（1目录 2菜单 3按钮）
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单类型")]
        public MenuTypeEnum Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称", Length = 50)]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [SugarColumn(ColumnDescription = "路由地址", Length = 100)]
        [MaxLength(100)]
        public string Path { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        [SugarColumn(ColumnDescription = "组件路径", Length = 100)]
        [MaxLength(100)]
        public string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        [SugarColumn(ColumnDescription = "权限标识", Length = 100)]
        [MaxLength(100)]
        public string Permission { get; set; }

        /// <summary>
        /// 重定向
        /// </summary>
        [SugarColumn(ColumnDescription = "重定向", Length = 100)]
        [MaxLength(100)]
        public string Redirect { get; set; }

        /// <summary>
        /// 内嵌地址
        /// </summary>
        [SugarColumn(ColumnDescription = "内嵌地址", Length = 100)]
        [MaxLength(200)]
        public string FrameSrc { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(ColumnDescription = "标题", Length = 50)]
        [Required, MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(ColumnDescription = "图标", Length = 100)]
        [MaxLength(100)]
        public string Icon { get; set; }

        /// <summary>
        /// 隐藏菜单
        /// </summary>
        [SugarColumn(ColumnDescription = "隐藏菜单")]
        public bool HideMenu { get; set; } = false;

        /// <summary>
        /// 忽略缓存
        /// </summary>
        [SugarColumn(ColumnDescription = "忽略缓存")]
        public bool IgnoreKeepAlive { get; set; } = false;

        /// <summary>
        /// 当前激活的菜单-用于配置详情页时左侧激活的菜单路径
        /// </summary>
        [SugarColumn(ColumnDescription = "当前激活菜单", Length = 100)]
        [MaxLength(100)]
        public string CurrentActiveMenu { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDescription = "排序")]
        public int OrderNo { get; set; }

        /// <summary>
        /// 菜单子项
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysMenu> Children { get; set; } = new List<SysMenu>();
    }
}