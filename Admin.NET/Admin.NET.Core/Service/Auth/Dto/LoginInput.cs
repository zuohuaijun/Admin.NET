namespace Admin.NET.Core.Service;

/// <summary>
/// 用户登录参数
/// </summary>
public class LoginInput
{
    /// <summary>
    /// 用户名
    /// </summary>
    /// <example>admin</example>
    [Required(ErrorMessage = "用户名不能为空"), MinLength(3, ErrorMessage = "用户名不能少于3个字符")]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <example>123456</example>
    [Required(ErrorMessage = "密码不能为空"), MinLength(3, ErrorMessage = "密码不能少于3个字符")]
    public string Password { get; set; }
}