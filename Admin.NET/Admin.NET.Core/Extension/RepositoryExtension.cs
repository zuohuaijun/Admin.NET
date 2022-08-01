namespace Admin.NET.Core;

/// <summary>
/// 仓储拓展
/// </summary>
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
        return repository.Context.FakeDelete(entity);
    }

    /// <summary>
    /// 实体假删除 db.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int FakeDelete<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Updateable(entity).ReSetValue(x => { x.IsDelete = true; })
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
        return repository.Context.FakeDeleteAsync(entity);
    }

    /// <summary>
    /// 实体假删除 db.FakeDelete(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> FakeDeleteAsync<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Updateable(entity).ReSetValue(x => { x.IsDelete = true; })
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
        return repository.Context.UpdateWithDiffLog(entity, ignoreAllNullColumns);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static int UpdateWithDiffLog<T>(this ISqlSugarClient db, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return db.Updateable(entity)
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
        return repository.Context.UpdateWithDiffLogAsync(entity, ignoreAllNullColumns);
    }

    /// <summary>
    /// 更新实体并记录差异日志 _rep.UpdateWithDiffLogAsync(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreAllNullColumns"></param>
    /// <returns></returns>
    public static Task<int> UpdateWithDiffLogAsync<T>(this ISqlSugarClient db, T entity, bool ignoreAllNullColumns = true) where T : EntityBase, new()
    {
        return db.Updateable(entity)
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
        return repository.Context.InsertWithDiffLog(entity);
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static int InsertWithDiffLog<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Insertable(entity).EnableDiffLogEvent().ExecuteCommand();
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
        return repository.Context.InsertWithDiffLogAsync(entity);
    }

    /// <summary>
    /// 新增实体并记录差异日志 _rep.InsertWithDiffLog(entity)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Task<int> InsertWithDiffLogAsync<T>(this ISqlSugarClient db, T entity) where T : EntityBase, new()
    {
        return db.Insertable(entity).EnableDiffLogEvent().ExecuteCommandAsync();
    }

    /// <summary>
    /// 多库查询
    /// </summary>
    /// <param name="queryable"></param>
    /// <returns> </returns>
    public static ISugarQueryable<T> AS<T>(this ISugarQueryable<T> queryable)
    {
        return queryable.AS<T>(GetTableName<T>(queryable.Context.Ado));
    }

    /// <summary>
    /// 多库查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="queryable"></param>
    /// <returns></returns>
    public static ISugarQueryable<T, T2> AS<T, T2>(this ISugarQueryable<T, T2> queryable)
    {
        return queryable.AS<T2>(GetTableName<T2>(queryable.Context.Ado));
    }

    /// <summary>
    /// 根据实体类型获取表信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetTableName<T>(IAdo ado)
    {
        var entityType = typeof(T);
        var attr = entityType.GetCustomAttribute<TenantAttribute>();
        var configId = attr == null ? SqlSugarConst.ConfigId : attr.configId.ToString();
        var tableName = entityType.GetCustomAttribute<SugarTable>().TableName;

        //根据实际的数据库类型 修改此处  如果固定使用一个数据库，可用直接写死
        var wholeTableName = $"{configId}.dbo.{tableName}";
        if (ado is MySqlProvider)
        {
            wholeTableName = $"{configId}.{tableName}";
        }
        else if (ado is SqlServerProvider)
        {
            wholeTableName = $"{configId}.dbo.{tableName}";
        }
        return wholeTableName;
    }
}