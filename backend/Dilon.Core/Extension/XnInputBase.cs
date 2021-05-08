using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 通用输入扩展参数（带权限）
    /// </summary>
    public interface IXnInputBase
    {
        /// <summary>
        /// 授权菜单
        /// </summary>
        public List<long> GrantMenuIdList { get; set; }

        /// <summary>
        /// 授权角色
        /// </summary>
        public List<long> GrantRoleIdList { get; set; }

        /// <summary>
        /// 授权数据
        /// </summary>
        public List<long> GrantOrgIdList { get; set; }
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

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string SortField { get; set; }

        /// <summary>
        /// 排序方法,默认升序,否则降序(配合antd前端,约定参数为 Ascend,Dscend)
        /// </summary>
        public virtual string SortOrder { get; set; }

        /// <summary>
        /// 降序排序(不要问我为什么是descend不是desc，前端约定参数就是这样)
        /// </summary>
        public virtual string DescStr { get; set; } = "descend";
    }
}