using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysOpLogService
    {
        Task ClearOpLog();

        Task<dynamic> QueryOpLogPageList([FromQuery] OpLogPageInput input);
    }
}