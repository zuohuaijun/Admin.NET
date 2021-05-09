using System.Collections;
using System.Collections.Generic;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 菜单树---授权、新增编辑时选择
    /// </summary>
    public class MenuTreeOutput : ITreeNode
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 排序，越小优先级越高
        /// </summary>
        public MenuWeight Weight { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuTreeOutput> Children { get; set; } = new List<MenuTreeOutput>();

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return ParentId;
        }

        public void SetChildren(IList children)
        {
            Children = (List<MenuTreeOutput>)children;
        }
    }
}