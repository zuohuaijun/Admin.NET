using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class MenuInput : BaseIdInput
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public virtual long Pid { get; set; }

        /// <summary>
        /// 菜单类型（1目录 2菜单 3按钮）
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        public virtual string Component { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public virtual string Permission { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public virtual string Redirect { get; set; }

        /// <summary>
        /// 内嵌地址
        /// </summary>
        public string FrameSrc { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int OrderNo { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual bool HideMenu { get; set; } = false;

        /// <summary>
        /// 是否忽略KeepAlive缓存
        /// </summary>
        public virtual bool IgnoreKeepAlive { get; set; } = true;

        /// <summary>
        /// 当前激活的菜单-用于配置详情页时左侧激活的菜单路径
        /// </summary>
        public virtual string CurrentActiveMenu { get; set; }
    }

    public class AddMenuInput : MenuInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "菜单名称不能为空")]
        public override string Title { get; set; }
    }

    public class UpdateMenuInput : AddMenuInput
    {
    }

    public class DeleteMenuInput : BaseIdInput
    {
    }
}