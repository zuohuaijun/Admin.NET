using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 增删改查基础服务
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityOutput"></typeparam>
    /// <typeparam name="TAddInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TPageListInput"></typeparam>
    /// <typeparam name="TDbContextLocator"></typeparam>
    public abstract class BaseService<TKey, TEntity, TEntityOutput, TAddInput, TUpdateInput, TPageListInput, TDbContextLocator>
        : IBaseService<TKey, TEntity, TEntityOutput, TAddInput, TUpdateInput, TPageListInput, TDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
        where TEntityOutput : class, IEntityOutput, new()
        where TAddInput : class, IAddInput, new()
        where TUpdateInput : class, IUpdateInput, new()
        where TPageListInput : class, IPageListInput, new()
        where TDbContextLocator : class, IDbContextLocator
    {
        public readonly IRepository<TEntity, TDbContextLocator> _rep;

        public BaseService()
        {
            _rep = Db.GetRepository<TEntity, TDbContextLocator>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[RequestSizeLimit(1000_000_000)]
        public virtual async Task<TEntityOutput> InsterAsync(TAddInput input)
        {
            var res = await _rep.InsertNowAsync(input.Adapt<TEntity>());
            return res.Entity.Adapt<TEntityOutput>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TUpdateInput input)
        {
            var res = await _rep.UpdateAsync(input.Adapt<TEntity>());
            return res.Entity != null;
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TKey key)
        {
            var res = await _rep.FakeDeleteAsync(key);
            return res.Entity != null;
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<TEntityOutput> GetAsync(TKey key)
        {
            var res = await _rep.FindOrDefaultAsync(key);
            return res.Adapt<TEntityOutput>();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntityOutput>> GetAllAsync()
        {
            var res = await _rep.AsQueryable(false).ToListAsync();
            return res.Adapt<List<TEntityOutput>>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<TEntityOutput>> GetPageAsync([FromQuery, Required] TPageListInput input)
        {
            var res = await _rep.AsQueryable(false).ToPagedListAsync(input.PageIndex, input.PageSize);
            return res.Adapt<PagedList<TEntityOutput>>();
        }

    }
}
