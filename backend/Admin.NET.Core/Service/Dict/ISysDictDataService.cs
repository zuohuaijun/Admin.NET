using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysDictDataService
    {
        Task AddDictData(AddDictDataInput input);

        Task ChangeDictDataStatus(ChageStateDictDataInput input);

        Task DeleteByTypeId(long dictTypeId);

        Task DeleteDictData(DeleteDictDataInput input);

        Task<dynamic> GetDictData([FromQuery] QueryDictDataInput input);

        Task<dynamic> GetDictDataList([FromQuery] QueryDictDataListInput input);

        Task<dynamic> GetDictDataListByDictTypeId(long dictTypeId);

        Task<dynamic> QueryDictDataPageList([FromQuery] DictDataPageInput input);

        Task UpdateDictData(UpdateDictDataInput input);
    }
}