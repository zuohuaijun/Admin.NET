using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ICommonService
    {
        Task<IEnumerable<EntityInfo>> GetEntityInfos();

        string GetHost();

        string GetFileUrl(SysFile sysFile);
    }
}