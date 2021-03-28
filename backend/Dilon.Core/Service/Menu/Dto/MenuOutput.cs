using System.Collections;
using System.Collections.Generic;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 菜单树（列表形式）
    /// </summary>
    public class MenuOutput : MenuInput, ITreeNode
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuOutput> Children { get; set; } = new List<MenuOutput>();

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return Pid;
        }

        public void SetChildren(IList children)
        {
            Children = (List<MenuOutput>)children;
        }
    }
}
