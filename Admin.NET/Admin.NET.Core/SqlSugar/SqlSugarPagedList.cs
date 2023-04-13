namespace Admin.NET.Core;

/// <summary>
/// 分页泛型集合
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class SqlSugarPagedList<TEntity>
    where TEntity : new()
{
    /// <summary>
    /// 页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 页容量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 当前页集合
    /// </summary>
    public IEnumerable<TEntity> Items { get; set; }

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPrevPage { get; set; }

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPage { get; set; }
}

/// <summary>
/// 分页拓展类
/// </summary>
public static class SqlSugarPagedExtensions
{
    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="query">ISugarQueryable 对象</param>
    /// <param name="pageIndex">当前页码，从1开始</param>
    /// <param name="pageSize">页码容量</param>
    /// <param name="expression">查询结果 Select 表达式</param>
    /// <returns></returns>
    public static SqlSugarPagedList<TResult> ToPagedList<TEntity, TResult>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize,
        Expression<Func<TEntity, TResult>> expression)
        where TEntity : new()
        where TResult : new()
    {
        var total = 0;
        var items = query.ToPageList(pageIndex, pageSize, ref total, expression);
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;
        return new SqlSugarPagedList<TResult>
        {
            Page = pageIndex,
            PageSize = pageSize,
            Items = items,
            Total = total,
            TotalPages = totalPages,
            HasNextPage = pageIndex < totalPages,
            HasPrevPage = pageIndex - 1 > 0
        };
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="query">ISugarQueryable 对象</param>
    /// <param name="pageIndex">当前页码，从1开始</param>
    /// <param name="pageSize">页码容量</param>
    /// <returns></returns>
    public static SqlSugarPagedList<TEntity> ToPagedList<TEntity>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize)
        where TEntity : new()
    {
        return ToPagedList(query, pageIndex, pageSize, u => u);
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="query">ISugarQueryable 对象</param>
    /// <param name="pageIndex">当前页码，从1开始</param>
    /// <param name="pageSize">页码容量</param>
    /// <param name="expression">查询结果 Select 表达式</param>
    /// <returns></returns>
    public static async Task<SqlSugarPagedList<TResult>> ToPagedListAsync<TEntity, TResult>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize,
        Expression<Func<TEntity, TResult>> expression)
        where TEntity : new()
        where TResult : new()
    {
        RefAsync<int> total = 0;
        var items = await query.ToPageListAsync(pageIndex, pageSize, total, expression);
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;
        return new SqlSugarPagedList<TResult>
        {
            Page = pageIndex,
            PageSize = pageSize,
            Items = items,
            Total = total,
            TotalPages = totalPages,
            HasNextPage = pageIndex < totalPages,
            HasPrevPage = pageIndex - 1 > 0
        };
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="query">ISugarQueryable 对象</param>
    /// <param name="pageIndex">当前页码，从1开始</param>
    /// <param name="pageSize">页码容量</param>
    /// <returns></returns>
    public static Task<SqlSugarPagedList<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize)
        where TEntity : new()
    {
        return ToPagedListAsync(query, pageIndex, pageSize, u => u);
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="list">集合对象</param>
    /// <param name="pageIndex">当前页码，从1开始</param>
    /// <param name="pageSize">页码容量</param>
    /// <returns></returns>
    public static SqlSugarPagedList<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> list, int pageIndex, int pageSize)
        where TEntity : new()
    {
        var total = list.Count();
        var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;
        return new SqlSugarPagedList<TEntity>
        {
            Page = pageIndex,
            PageSize = pageSize,
            Items = items,
            Total = total,
            TotalPages = totalPages,
            HasNextPage = pageIndex < totalPages,
            HasPrevPage = pageIndex - 1 > 0
        };
    }
}