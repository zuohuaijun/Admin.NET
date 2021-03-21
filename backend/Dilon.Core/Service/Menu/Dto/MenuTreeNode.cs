using System.Collections;
using System.Collections.Generic;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 菜单树
    /// </summary>
    public class MenuTreeNode : TreeNodeBase
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
        public int Weight { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuTreeNode> Children { get; set; } = new List<MenuTreeNode>();

        public override long GetId()
        {
            return Id;
        }

        public override long GetPid()
        {
            return ParentId;
        }

        public override void SetChildren(IList children)
        {
            Children = (List<MenuTreeNode>)children;
        }
    }
}
