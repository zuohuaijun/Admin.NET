using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 登陆类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登陆")]
        LOGIN = 0,

        /// <summary>
        /// 登出
        /// </summary>
        [Description("登出")]
        LOGOUT = 1,

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        REGISTER = 2,

        /// <summary>
        /// 改密
        /// </summary>
        [Description("改密")]
        CHANGEPASSWORD = 3,

        /// <summary>
        /// 三方授权登陆
        /// </summary>
        [Description("授权登陆")]
        AUTHORIZEDLOGIN = 4
    }
}