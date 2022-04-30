namespace Admin.NET.Core
{
    /// <summary>
    /// 全局分页查询输入参数
    /// </summary>
    public class BasePageInput
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public virtual int Page { get; set; }

        /// <summary>
        /// 页码容量
        /// </summary>
        public virtual int PageSize { get; set; }
    }
}