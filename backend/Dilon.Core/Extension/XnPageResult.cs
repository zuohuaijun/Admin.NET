using System.Collections.Generic;
using Mapster;

namespace Dilon.Core
{
    //public class PageResult<T>
    //{
    //    public int PageNo { get; set; }
    //    public int PageSize { get; set; }
    //    public int TotalPage { get; set; }
    //    public int TotalRows { get; set; }
    //    public ICollection<T> Rows { get; set; }
    //}

    /// <summary>
    /// 小诺分页列表结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class XnPageResult<T> where T : new()
    {
        public static dynamic PageResult(PagedList<T> page)
        {
            return new
            {
                PageNo = page.PageIndex,
                PageSize = page.PageSize,
                TotalPage = page.TotalPages,
                TotalRows = page.TotalCount,
                Rows = page.Items //.Adapt<List<T>>(),
                //Rainbow = PageUtil.Rainbow(page.PageIndex, page.TotalPages, PageUtil.RAINBOW_NUM)
            };
        }

        //public static PageResult<V> PageResult<V>(PagedList<T> page)
        //{
        //    return new PageResult<V>
        //    {
        //        PageNo = page.PageIndex,
        //        PageSize = page.PageSize,
        //        TotalPage = page.TotalPages,
        //        TotalRows = page.TotalCount,
        //        Rows = page.Items.Adapt<List<V>>(),
        //    };
        //}
    }
}
