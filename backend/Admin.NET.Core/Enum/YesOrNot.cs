using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 菜单激活类型
    /// </summary>
    public enum YesOrNot
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Y = 0,

        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        N = 1
    }
}