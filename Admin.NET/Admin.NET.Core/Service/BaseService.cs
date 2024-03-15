// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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
    /// 获取详情 🔖
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("获取详情")]
    public virtual async Task<TEntity> GetDetail(long id)
    {
        return await _rep.GetByIdAsync(id);
    }

    /// <summary>
    /// 获取集合 🔖
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取集合")]
    public virtual async Task<List<TEntity>> GetList()
    {
        return await _rep.GetListAsync();
    }

    ///// <summary>
    ///// 获取实体分页 🔖
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
    /// 增加 🔖
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加")]
    public virtual async Task<bool> Add(TEntity entity)
    {
        return await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 更新 🔖
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新")]
    public virtual async Task<bool> Update(TEntity entity)
    {
        return await _rep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除 🔖
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除")]
    public virtual async Task<bool> Delete(long id)
    {
        return await _rep.DeleteByIdAsync(id);
    }
}