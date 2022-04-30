using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 账号类型枚举
    /// </summary>
    public enum UserTypeEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 1,

        /// <summary>
        /// 普通账号
        /// </summary>
        [Description("普通账号")]
        None = 2,

        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 999,
    }
}