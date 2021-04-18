using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dilon.Core.Service
{
    public interface ISysEnumDataService
    {
        Task<dynamic> GetEnumDataList([FromQuery]QueryEnumDataListInput input);
        Task<dynamic> GetEnumDataListByField([FromQuery] QueryEnumDataListByFiledInput input);
    }
}