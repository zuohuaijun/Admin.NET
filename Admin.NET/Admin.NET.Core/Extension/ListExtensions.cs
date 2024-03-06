// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

public static class ListExtensions
{
    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        foreach (var value in list)
        {
            await func(value);
        }
    }

    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
    {
        foreach (var value in source)
        {
            await action(value);
        }
    }
}