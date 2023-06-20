namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// 登录输入
/// </summary>
public class GoViewLoginInput
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; }
}