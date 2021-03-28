using System.Collections;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 树基类
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// 获取节点id
        /// </summary>
        /// <returns></returns>
        long GetId();

        /// <summary>
        /// 获取节点父id
        /// </summary>
        /// <returns></returns>
        long GetPid();

        /// <summary>
        /// 设置Children
        /// </summary>
        /// <param name="children"></param>
        void SetChildren(IList children);
    }

    /// <summary>
    /// 递归工具类，用于遍历有父子关系的节点，例如菜单树，字典树等等
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeBuildUtil<T> where T : ITreeNode
    {
        /// <summary>
        /// 顶级节点的父节点Id(默认0)
        /// </summary>
        private readonly long _rootParentId = 0L;

        /// <summary>
        /// 构造树节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<T> DoTreeBuild(List<T> nodes)
        {
            nodes.ForEach(u => BuildChildNodes(nodes, u, new List<T>()));

            var results = new List<T>();
            nodes.ForEach(u =>
            {
                if (_rootParentId == u.GetPid())
                    results.Add(u);
            });
            return results;
        }

        /// <summary>
        /// 构造子节点集合
        /// </summary>
        /// <param name="totalNodes"></param>
        /// <param name="node"></param>
        /// <param name="childNodeLists"></param>
        private void BuildChildNodes(List<T> totalNodes, T node, List<T> childNodeLists)
        {
            var nodeSubLists = new List<T>();
            totalNodes.ForEach(u =>
            {
                if (u.GetPid().Equals(node.GetId()))
                    nodeSubLists.Add(u);
            });
            nodeSubLists.ForEach(u => BuildChildNodes(totalNodes, u, new List<T>()));
            childNodeLists.AddRange(nodeSubLists);
            node.SetChildren(childNodeLists);
        }
    }
}
