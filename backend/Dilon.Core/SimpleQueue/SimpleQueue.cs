using System.Collections.Concurrent;

namespace Dilon.Core
{
    /// <summary>
    /// 简单泛型队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleQueue<T> : IConcurrentQueue<T>
    {
        private static ConcurrentQueue<T> _simpleQueue;

        public SimpleQueue()
        {
            _simpleQueue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        public void Add(T obj)
        {
            _simpleQueue.Enqueue(obj);
        }

        /// <summary>
        /// 取出
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Try(out T obj)
        {
            return _simpleQueue.TryDequeue(out obj);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _simpleQueue.Count;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _simpleQueue.Clear();
        }
    }
}