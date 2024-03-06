// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 实体种子数据接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISqlSugarEntitySeedData<TEntity>
    where TEntity : class, new()
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    IEnumerable<TEntity> HasData();
}