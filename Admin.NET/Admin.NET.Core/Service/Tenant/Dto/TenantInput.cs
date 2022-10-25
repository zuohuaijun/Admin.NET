namespace Admin.NET.Core.Service;

public class TenantInput : BaseIdInput
{
}

public class PageTenantInput : BasePageInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public virtual string Phone { get; set; }
}

[NotTable]
public class AddTenantInput : SysTenant
{
    /// <summary>
    /// 租户名称
    /// </summary>
    [Required(ErrorMessage = "租户名称不能为空"), MinLength(2, ErrorMessage = "租户名称不能少于2个字符")]
    public override string Name { get; set; }

    /// <summary>
    /// 管理员名称
    /// </summary>
    [Required(ErrorMessage = "管理员名称不能为空"), MinLength(3, ErrorMessage = "管理员名称不能少于3个字符")]
    public override string AdminName { get; set; }
}

[NotTable]
public class UpdateTenantInput : AddTenantInput
{
}

public class DeleteTenantInput : BaseIdInput
{
}