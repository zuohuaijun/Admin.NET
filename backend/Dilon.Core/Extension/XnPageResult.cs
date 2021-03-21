using System.Collections.Generic;

namespace Dilon.Core
{
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
    }
}
