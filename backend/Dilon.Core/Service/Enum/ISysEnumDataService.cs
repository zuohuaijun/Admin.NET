using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysEnumDataService
    {
        Task<dynamic> GetEnumDataList([FromQuery] EnumDataInput input);

        Task<dynamic> GetEnumDataListByField([FromQuery] QueryEnumDataInput input);
    }
}