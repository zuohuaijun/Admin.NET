namespace Admin.NET.Core.Service;

[NotTable]
public class MenuInput : SysMenu
{

}

[NotTable]
public class AddMenuInput : MenuInput
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "菜单名称不能为空")]
    public override string Title { get; set; }
}

[NotTable]
public class UpdateMenuInput : AddMenuInput
{
}

public class DeleteMenuInput : BaseIdInput
{
}