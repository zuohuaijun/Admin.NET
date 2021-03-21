using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 通用输入扩展参数（带权限）
    /// </summary>
    public class XnInputBase : PageInputBase
    {
        /// <summary>
        /// 授权菜单
        /// </summary>
        public List<long> GrantMenuIdList { get; set; } = new List<long>();

        /// <summary>
        /// 授权角色
        /// </summary>
        public virtual List<long> GrantRoleIdList { get; set; } = new List<long>();

        /// <summary>
        /// 授权数据
        /// </summary>
        public virtual List<long> GrantOrgIdList { get; set; } = new List<long>();
    }

    /// <summary>
    /// 通用分页输入参数
    /// </summary>
    public class PageInputBase
    {
        /// <summary>
        /// 搜索值
        /// </summary>
        public virtual string SearchValue { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public virtual int PageNo { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        public virtual int PageSize { get; set; } = 20;

        /// <summary>
        /// 搜索开始时间
        /// </summary>
        public virtual string SearchBeginTime { get; set; }

        /// <summary>
        /// 搜索结束时间
        /// </summary>
        public virtual string SearchEndTime { get; set; }
    }
}
