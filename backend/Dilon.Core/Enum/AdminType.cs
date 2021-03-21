using System.ComponentModel;

namespace Dilon.Core
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AdminType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 1,

        /// <summary>
        /// 非管理员
        /// </summary>
        [Description("非管理员")]
        None = 2
    }
}
