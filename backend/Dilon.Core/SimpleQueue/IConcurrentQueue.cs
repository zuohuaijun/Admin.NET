namespace Dilon.Core
{
    public interface IConcurrentQueue<T>
    {
        void Add(T obj);
        bool Try(out T obj);
        int Count();
        void Clear();
    }
}