namespace Dilon.Core.Service
{
    public interface IPageListInput
    {
        /// <summary>
        /// 当前页面
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数据行数
        /// </summary>
        public int PageSize { get; set; }
    }
}
