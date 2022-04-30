using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 登录类型枚举
    /// </summary>
    public enum LoginTypeEnum
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login,

        /// <summary>
        /// 退出
        /// </summary>
        [Description("退出")]
        Logout,

        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register,

        /// <summary>
        /// 改密
        /// </summary>
        [Description("改密")]
        Change_password,

        /// <summary>
        /// 三方授权登陆
        /// </summary>
        [Description("授权登陆")]
        Authorized_login
    }
}