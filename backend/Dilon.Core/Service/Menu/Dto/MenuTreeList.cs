using System.Collections;
using System.Collections.Generic;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 菜单树（列表形式）
    /// </summary>
    public class MenuTreeList : MenuInput
    {
        /// <summary>
        /// 子节点
        /// </summary>
        public List<MenuTreeList> Children { get; set; } = new List<MenuTreeList>();

        public override long GetId()
        {
            return long.Parse(Id);
        }

        public override long GetPid()
        {
            return long.Parse(Pid);
        }

        public override void SetChildren(IList children)
        {
            Children = (List<MenuTreeList>)children;
        }
    }
}
