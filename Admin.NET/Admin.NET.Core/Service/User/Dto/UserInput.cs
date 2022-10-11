namespace Admin.NET.Core.Service;

public class UserInput : BaseIdInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public virtual StatusEnum Status { get; set; }
}

public class PageUserInput : BasePageInput
{
    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }
}

[NotTable]
public class AddUserInput : SysUser
{
    /// <summary>
    /// 账号名称
    /// </summary>
    [Required(ErrorMessage = "账号名称不能为空")]
    public override string UserName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Required(ErrorMessage = "真实姓名不能为空")]
    public override string RealName { get; set; }

    /// <summary>
    /// 账号密码
    /// </summary>
    public override string Password { get; set; } = CommonConst.SysPassword;
}

[NotTable]
public class UpdateUserInput : AddUserInput
{
}

public class DeleteUserInput : BaseIdInput
{
    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }
}

public class ResetPwdUserInput : BaseIdInput
{
}

public class ChangePwdInput
{
    /// <summary>
    /// 当前密码
    /// </summary>
    [Required(ErrorMessage = "当前密码不能为空")]
    public string PasswordOld { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "原始密码不能为空")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
    public string PasswordNew { get; set; }
}