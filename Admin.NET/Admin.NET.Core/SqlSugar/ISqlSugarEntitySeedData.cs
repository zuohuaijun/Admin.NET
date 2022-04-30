using System.Collections.Generic;

namespace Admin.NET.Core
{
    public interface ISqlSugarEntitySeedData<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 配置种子数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> HasData();
    }
}