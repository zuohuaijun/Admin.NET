using Furion.FriendlyException;
using Mapster;
using MapsterMapper;

namespace Admin.NET.Core
{
    /// <summary>
    /// 列表通用排序
    /// </summary>
    public static class PageInputOrder
    {
        /// <summary> 
        /// 排序方式(默认降序)
        /// </summary>
        /// <param name="pageInput"> </param>
        /// <param name="defualtSortField"> 默认排序字段 </param>
        /// <param name="descSort"> 是否降序 </param>
        /// <returns> </returns>
        public static string OrderBuilder<T>(T pageInput, string defualtSortField = "Id", bool descSort = true)
        {
            string orderStr;
            // 约定默认每张表都有Id排序
            if (!string.IsNullOrWhiteSpace(defualtSortField))
                orderStr = descSort ? defualtSortField + " Desc" : defualtSortField + " Asc";
            else
                throw Oops.Oh("排序字段不能为空！");

            TypeAdapterConfig config = new();
            config.ForType<T, PageInputBase>().IgnoreNullValues(true);
            Mapper mapper = new(config); // 务必将mapper设为单实例
            PageInputBase nowPagerInput = mapper.Map<PageInputBase>(pageInput); ;
            // 排序是否可用-排序字段和排序顺序都为非空才启用排序
            if (!string.IsNullOrEmpty(nowPagerInput.SortField) && !string.IsNullOrEmpty(nowPagerInput.SortOrder))
            {
                orderStr = $"{nowPagerInput.SortField} {(nowPagerInput.SortOrder == nowPagerInput.DescStr ? "Desc" : "Asc")}";
            }
            return orderStr;
        }
    }
}