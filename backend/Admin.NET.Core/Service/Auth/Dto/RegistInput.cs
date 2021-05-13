using Furion.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service.Auth.Dto
{
    /// <summary>
    /// 注册输入参数
    /// </summary>
    public class RegistInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "用户名不能为空"), MinLength(3, ErrorMessage = "用户名不能少于3位字符")]
        public string Email { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "公司不能为空"), MinLength(3, ErrorMessage = "公司不能少于3位字符")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "密码不能为空"), MinLength(5, ErrorMessage = "密码不能少于5位字符")]
        public string Password { get; set; }
    }
}