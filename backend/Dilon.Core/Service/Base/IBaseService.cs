using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface IBaseService<TKey, TEntity, TEntityOutput, TAddInput, TUpdateInput, TPageListInput, TDbContextLocator>
        where TEntityOutput : class, IEntityOutput, new()
        where TAddInput : class, IAddInput, new()
        where TUpdateInput : class, IUpdateInput, new()
        where TPageListInput : class, IPageListInput, new()
    {
        Task<bool> DeleteAsync(TKey key);
        Task<IEnumerable<TEntityOutput>> GetAllAsync();
        Task<TEntityOutput> GetAsync(TKey key);
        Task<PagedList<TEntityOutput>> GetPageAsync([FromQuery, Required] TPageListInput input);
        Task<TEntityOutput> InsterAsync(TAddInput input);
        Task<bool> UpdateAsync(TUpdateInput input);
    }
}