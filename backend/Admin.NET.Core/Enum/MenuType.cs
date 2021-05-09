using System.ComponentModel;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统菜单类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        DIR = 0,

        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        MENU = 1,

        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        BTN = 2
    }
}