using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Core
{
    /// <summary>
    /// 通用输入帮助类
    /// </summary>
    public class PageInputHelp
    {
        /// <summary>
        /// 排序方式
        /// 默认降序
        /// </summary>
        /// <param name="pageInput"></param>
        /// <param name="descSort">是否降序</param>
        /// <returns></returns>
        public static String OrderBuilder(PageInputBase pageInput, Boolean descSort = true)
        {

            //约定，每张表都有id排序
            var orderStr = "Id Desc";
            if (!descSort)
            {
                orderStr = "Id Asc";
            }

            // 排序是否可用
            // 排序字段和排序顺序都为非空才启用排序
            if (!string.IsNullOrEmpty(pageInput.SortField) && !string.IsNullOrEmpty(pageInput.SortOrder))
            {
                orderStr = $"{pageInput.SortField} {(pageInput.SortOrder == pageInput.DescStr ? "Desc" : "Asc")}";
            }

            return orderStr;
        }
    }
}
