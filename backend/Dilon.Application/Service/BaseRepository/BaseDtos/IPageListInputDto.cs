using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{
    public interface IPageListInputDto
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
