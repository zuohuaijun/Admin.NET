using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Covid19.Plugin
{
    public interface IXgCollectorService
    {
        Task AddXgCollector(AddXgCollectorInput input);
        Task DeleteXgCollector(DeleteXgCollectorInput input);
        Task<XgCollector> GetXgCollector([FromQuery] QueryXgCollectorInput input);
        Task<dynamic> QueryXgCollectorPageList([FromQuery] XgCollectorInput input);
        Task UpdateXgCollector(UpdateXgCollectorInput input);
    }
}