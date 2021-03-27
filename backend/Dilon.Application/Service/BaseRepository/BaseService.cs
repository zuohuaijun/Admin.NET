using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace Dilon.Application
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityOutputDto"></typeparam>
    /// <typeparam name="TAddInputDto"></typeparam>
    /// <typeparam name="TUpdateInputDto"></typeparam>
    /// <typeparam name="TPageListInputDto"></typeparam>
    /// <typeparam name="TDbContextLocator"></typeparam>
    [SkipScan]
    public abstract class BaseService<TKey, TEntity, TEntityOutputDto, TAddInputDto, TUpdateInputDto
        , TPageListInputDto, TDbContextLocator>
        : IBaseService<TKey, TEntity, TEntityOutputDto, TAddInputDto, TUpdateInputDto
            , TPageListInputDto, TDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
        where TEntityOutputDto : class, IEntityOutputDto, new()//查询返回
        where TAddInputDto : class, IAddInputDto, new()//添加dto
        where TUpdateInputDto : class, IUpdateInputDto, new()//编辑dto
        where TPageListInputDto : class, IPageListInputDto, new()//分页查询
        where TDbContextLocator : class, IDbContextLocator
    {
        public readonly IRepository<TEntity, TDbContextLocator> _repository;

        public BaseService()
        {
            _repository = Db.GetRepository<TEntity, TDbContextLocator>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[RequestSizeLimit(1000_000_000)]
        public virtual async Task<TEntityOutputDto> InsterAsync(TAddInputDto input)
        {
            var tentity = input.Adapt<TEntity>();
            var res = await _repository.InsertNowAsync(tentity);
            return res.Entity.Adapt<TEntityOutputDto>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TUpdateInputDto input)
        {
            var tentity = input.Adapt<TEntity>();
            var res = await _repository.UpdateAsync(tentity);
            return res.Entity == null ? false : true;
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TKey key)
        {
            var res = await _repository.FakeDeleteAsync(key);
            return res.Entity == null ? false : true;
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<TEntityOutputDto> GetAsync(TKey key)
        {
            var res = await _repository.FindOrDefaultAsync(key);
            return res.Adapt<TEntityOutputDto>();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntityOutputDto>> GetAllAsync()
        {
            var res = await _repository.AsAsyncEnumerable(false);
            return res.Adapt<List<TEntityOutputDto>>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<TEntityOutputDto>> GetPageAsync([FromQuery, Required] TPageListInputDto input)
        {
            var res = await _repository.AsQueryable(false).ToPagedListAsync(input.PageIndex, input.PageSize);
            return res.Adapt<PagedList<TEntityOutputDto>>();
        }

    }
}
