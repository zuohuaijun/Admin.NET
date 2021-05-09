using System.ComponentModel;

namespace Admin.NET.Core
{
    public enum DataScopeType
    {
        /// <summary>
        /// 全部数据
        /// </summary>
        [Description("全部数据")]
        ALL = 1,

        /// <summary>
        /// 本部门及以下数据
        /// </summary>
        [Description("本部门及以下数据")]
        DEPT_WITH_CHILD = 2,

        /// <summary>
        /// 本部门数据
        /// </summary>
        [Description("本部门数据")]
        DEPT = 3,

        /// <summary>
        /// 仅本人数据
        /// </summary>
        [Description("仅本人数据")]
        SELF = 4,

        /// <summary>
        /// 自定义数据
        /// </summary>
        [Description("自定义数据")]
        DEFINE = 5
    }
}