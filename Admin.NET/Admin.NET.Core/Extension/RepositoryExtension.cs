namespace Admin.NET.Core;

public static class RepositoryExtension
{
    /// <summary>
    /// 实体假删除 _rep.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int FakeDelete<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.Updateable(entity).ReSetValue(x => { x.IsDelete = true; })
            .IgnoreColumns(ignoreAllNullColumns: true)
            .EnableDiffLogEvent()   // 记录差异日志
            .UpdateColumns(x => new { x.IsDelete, x.UpdateTime, x.UpdateUserId })  // 允许更新的字段-AOP拦截自动设置UpdateTime、UpdateUserId
            .ExecuteCommand();
    }

    /// <summary>
    /// 实体假删除异步 _rep.FakeDeleteAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> FakeDeleteAsync<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.Updateable(entity).ReSetValue(x => { x.IsDelete = true; })
            .IgnoreColumns(ignoreAllNullColumns: true)
            .EnableDiffLogEvent()   // 记录差异日志
            .UpdateColumns(x => new { x.IsDelete, x.UpdateTime, x.UpdateUserId })  // 允许更新的字段-AOP拦截自动设置UpdateTime、UpdateUserId
            .ExecuteCommandAsync();
    }


    /// <summary>
    /// 排序方式(默认降序)
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="pageInput"> </param>
    /// <param name="defualtSortField"> 默认排序字段 </param>
    /// <param name="descSort"> 是否降序 </param>
    /// <returns> </returns>
    public static ISugarQueryable<T> OrderBuilder<T>(this ISugarQueryable<T> queryable, BasePageInput pageInput, string defualtSortField = "Id", bool descSort = true)
    {
        var orderStr = "";
        // 约定默认每张表都有Id排序
        if (!string.IsNullOrWhiteSpace(defualtSortField))
        {
            orderStr = descSort ? defualtSortField + " Desc" : defualtSortField + " Asc";
        }
        TypeAdapterConfig config = new();
        config.ForType<T, BasePageInput>().IgnoreNullValues(true);
        Mapper mapper = new(config); // 务必将mapper设为单实例
        var nowPagerInput = mapper.Map<BasePageInput>(pageInput);
        // 排序是否可用-排序字段和排序顺序都为非空才启用排序
        if (!string.IsNullOrEmpty(nowPagerInput.Field) && !string.IsNullOrEmpty(nowPagerInput.Order))
        {
            orderStr = $"{nowPagerInput.Field} {(nowPagerInput.Order == nowPagerInput.DescStr ? "Desc" : "Asc")}";
        }
        return queryable.OrderByIF(!string.IsNullOrWhiteSpace(orderStr), orderStr);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static int UpdateWithDiffLog<T>(this ISugarRepository repository, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return repository.Context.Updateable(entity)
            .IgnoreColumns(ignoreAllNullColumns: ignoreAllNullColumns)
            .EnableDiffLogEvent()
            .ExecuteCommand();
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static Task<int> UpdateWithDiffLogAsync<T>(this ISugarRepository repository, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return repository.Context.Updateable(entity)
            .IgnoreColumns(ignoreAllNullColumns: ignoreAllNullColumns)
            .EnableDiffLogEvent()
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int InsertWithDiffLog<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.Insertable(entity)
            .EnableDiffLogEvent()
            .ExecuteCommand();
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="repository"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> InsertWithDiffLogAsync<T>(this ISugarRepository repository, T entity) where T : EntityBase, new()
    {
        return repository.Context.Insertable(entity)
            .EnableDiffLogEvent()
            .ExecuteCommandAsync();
    }
}