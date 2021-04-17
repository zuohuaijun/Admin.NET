using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysExLogService
    {
        Task ClearExLog();
        Task<dynamic> QueryExLogPageList([FromQuery] ExLogInput input);
    }
}