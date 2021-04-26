namespace Dilon.Core
{
    /// <summary>
    /// 通用输入帮助类
    /// </summary>
    public class PageInputOrder
    {
        /// <summary>
        /// 排序方式(默认降序)
        /// </summary>
        /// <param name="pageInput"></param>
        /// <param name="descSort">是否降序</param>
        /// <returns></returns>
        public static string OrderBuilder(PageInputBase pageInput, bool descSort = true)
        {
            // 约定默认每张表都有Id排序
            var orderStr = descSort ? "Id Desc" : "Id Asc";

            // 排序是否可用-排序字段和排序顺序都为非空才启用排序
            if (!string.IsNullOrEmpty(pageInput.SortField) && !string.IsNullOrEmpty(pageInput.SortOrder))
            {
                orderStr = $"{pageInput.SortField} {(pageInput.SortOrder == pageInput.DescStr ? "Desc" : "Asc")}";
            }
            return orderStr;
        }
    }
}