using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 菜单参数
    /// </summary>
    public class MenuInput
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public virtual long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 菜单类型（字典 0目录 1菜单 2按钮）
        /// </summary>
        public virtual MenuType Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public virtual string Router { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public virtual string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public virtual string Permission { get; set; }

        /// <summary>
        /// 应用分类（应用编码）
        /// </summary>
        public virtual string Application { get; set; }

        /// <summary>
        /// 打开方式（字典 0无 1组件 2内链 3外链）
        /// </summary>
        public virtual MenuOpenType OpenType { get; set; }

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
        public MenuWeight Weight { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class AddMenuInput : MenuInput
    {
        /// <summary>
        /// 菜单类型（字典 0目录 1菜单 2按钮）
        /// </summary>
        [Required(ErrorMessage = "菜单类型不能为空")]
        public override MenuType Type { get; set; }
    }

    public class DeleteMenuInput
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required(ErrorMessage = "菜单Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateMenuInput : AddMenuInput
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required(ErrorMessage = "菜单Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>DeleteMenuInput
        [Required(ErrorMessage = "父级菜单Id不能为空")]
        public override long Pid { get; set; }
    }

    public class QueryMenuInput : DeleteMenuInput
    {
    }

    public class ChangeAppMenuInput : MenuInput
    {
        /// <summary>
        /// 应用编码
        /// </summary>DeleteMenuInput
        [Required(ErrorMessage = "应用编码不能为空")]
        public override string Application { get; set; }
    }
}