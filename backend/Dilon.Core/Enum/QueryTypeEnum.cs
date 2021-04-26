using System.ComponentModel;

namespace Dilon.Core
{
    /// <summary>
    /// 查询类型的枚举
    /// </summary>
    public enum QueryTypeEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        eq = 0,

        /// <summary>
        /// 模糊
        /// </summary>
        [Description("模糊")]
        like = 1,

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        gt = 2,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        lt = 3,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        ne = 4,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        ge = 5,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        le = 6,

        /// <summary>
        /// 不为空
        /// </summary>
        [Description("不为空")]
        isNotNull = 7
    }
}