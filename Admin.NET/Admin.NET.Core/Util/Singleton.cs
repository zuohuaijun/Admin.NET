namespace Admin.NET.Core;

/// <summary>
/// 单例泛型类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class
{
    private static readonly Lazy<T> _instance
        = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(
                BindingFlags.Instance
                | BindingFlags.NonPublic
                | BindingFlags.Public);
            if (ctors.Count() != 1)
                throw new InvalidOperationException($"Type {typeof(T)} must have exactly one constructor.");
            var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any() && c.IsPrivate);
            if (ctor == null)
                throw new InvalidOperationException(
                    $"The constructor for {typeof(T)} must be private and take no parameters.");
            return (T)ctor.Invoke(null);
        });

    public static T Instance => _instance.Value;
}