using System.Collections.Concurrent;
using Furion.DatabaseAccessor;

namespace Dilon.Core
{
    /// <summary>
    /// 简单泛型队列
    /// </summary>
    public static class SimpleQueue<T> where T : EntityBase
    {
        private static ConcurrentQueue<T> _simpleQueue;

        static SimpleQueue()
        {
            _simpleQueue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        public static void Add(T obj)
        {
            _simpleQueue.Enqueue(obj);
        }

        /// <summary>
        /// 取出
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Try(out T obj)
        {
            return _simpleQueue.TryDequeue(out obj);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return _simpleQueue.Count;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Clear()
        {
            _simpleQueue.Clear();
        }
    }
}