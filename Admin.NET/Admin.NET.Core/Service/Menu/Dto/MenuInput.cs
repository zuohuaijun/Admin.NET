// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class MenuInput
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 菜单类型（1目录 2菜单 3按钮）
    /// </summary>
    public MenuTypeEnum? Type { get; set; }
}

public class AddMenuInput : SysMenu
{
    /// <summary>
    /// 名称
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