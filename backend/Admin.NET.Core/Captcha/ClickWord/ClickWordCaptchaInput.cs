using Furion.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 点击验证码输入参数
    /// </summary>
    public class ClickWordCaptchaInput
    {
        /// <summary>
        /// 验证码类型
        /// </summary>
        [Required(ErrorMessage = "验证码类型")]
        public string CaptchaType { get; set; }

        /// <summary>
        /// 坐标点集合
        /// </summary>
        [Required(ErrorMessage = "坐标点集合不能为空")]
        public string PointJson { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}