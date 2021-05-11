using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Covid19.Plugin
{
    public interface IXgTestItemService
    {
        Task AddXgTestItem(AddXgTestItemInput input);
        Task DeleteXgTestItem(DeleteXgTestItemInput input);
        Task<XgTestItem> GetXgTestItem([FromQuery] QueryXgTestItemInput input);
        Task<dynamic> QueryXgTestItemPageList([FromQuery] XgTestItemInput input);
        Task UpdateXgTestItem(UpdateXgTestItemInput input);
    }
}