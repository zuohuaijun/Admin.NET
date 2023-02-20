namespace Admin.NET.Core.Service;

/// <summary>
/// 自定义方法
/// </summary>
public class FuncList
{
    /// <summary>
    /// 字符串相加
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public string Merge(object a, object b)
    {
        return a.ToString() + b.ToString();
    }

    /// <summary>
    /// 对象合并
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public object MergeObj(object a, object b)
    {
        return new { a, b };
    }

    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsContain(object a, object b)
    {
        return a.ToString().Split(',').Contains(b);
    }
}