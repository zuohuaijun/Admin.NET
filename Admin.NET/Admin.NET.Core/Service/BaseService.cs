namespace Admin.NET.Core;

/// <summary>
/// 实体操作基服务
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseService<TEntity> : IDynamicApiController where TEntity : class, new()
{
    private readonly SqlSugarRepository<TEntity> _rep;

    public BaseService(SqlSugarRepository<TEntity> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 获取实体详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("获取实体详情")]
    public virtual async Task<TEntity> GetDetail(long id)
    {
        return await _rep.GetByIdAsync(id);
    }

    /// <summary>
    /// 获取实体集合
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取实体集合")]
    public virtual async Task<List<TEntity>> GetList()
    {
        return await _rep.GetListAsync();
    }

    ///// <summary>
    ///// 获取实体分页
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[ApiDescriptionSettings(Name = "Page")]
    //[DisplayName("获取实体分页")]
    //public async Task<SqlSugarPagedList<TEntity>> GetPage([FromQuery] BasePageInput input)
    //{
    //    return await _rep.AsQueryable().ToPagedListAsync(input.Page, input.PageSize);
    //}

    /// <summary>
    /// 增加实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加实体")]
    public virtual async Task<bool> Add(TEntity entity)
    {
        return await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新实体")]
    public virtual async Task<bool> Update(TEntity entity)
    {
        return await _rep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除实体")]
    public virtual async Task<bool> Delete(long id)
    {
        return await _rep.DeleteByIdAsync(id);
    }
}