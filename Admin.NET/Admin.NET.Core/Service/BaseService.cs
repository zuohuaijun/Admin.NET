// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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