using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{
    public interface IBaseService<TKey, TEntity, TEntityOutputDto, TAddInputDto, TUpdateInputDto
        , TPageListInputDto, TDbContextLocator>
            where TEntity : class, IPrivateEntity, new()
            where TEntityOutputDto : class, IEntityOutputDto, new()//查询返回
            where TAddInputDto : class, IAddInputDto, new()//添加dto
            where TUpdateInputDto : class, IUpdateInputDto, new()//编辑dto
            where TPageListInputDto : class, IPageListInputDto, new()//分页查询
            where TDbContextLocator : class, IDbContextLocator
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityOutputDto> InsterAsync(TAddInputDto input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TUpdateInputDto input);
        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TKey key);
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TEntityOutputDto> GetAsync(TKey key);
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntityOutputDto>> GetAllAsync();
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedList<TEntityOutputDto>> GetPageAsync([FromQuery, Required] TPageListInputDto input);
    }
}
