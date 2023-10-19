namespace Admin.NET.Core.Service;

/// <summary>
/// 开放接口身份输入参数
/// </summary>
public class OpenAccessInput : BasePageInput
{
    /// <summary>
    /// 身份标识
    /// </summary>
    public string AccessKey { get; set; }
}

public class AddOpenAccessInput : SysOpenAccess
{
    /// <summary>
    /// 身份标识
    /// </summary>
    [Required(ErrorMessage = "身份标识不能为空")]
    public override string AccessKey { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    [Required(ErrorMessage = "密钥不能为空")]
    public override string AccessSecret { get; set; }

    /// <summary>
    /// 绑定用户Id
    /// </summary>
    [Required(ErrorMessage = "绑定用户不能为空")]
    public override long BindUserId { get; set; }
}

public class UpdateOpenAccessInput : AddOpenAccessInput
{
}

public class DeleteOpenAccessInput : BaseIdInput
{
}
