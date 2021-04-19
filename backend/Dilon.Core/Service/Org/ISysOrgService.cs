using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysOrgService
    {
        Task AddOrg(AddOrgInput input);
        Task DeleteOrg(DeleteOrgInput input);
        Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId);
        Task<SysOrg> GetOrg([FromQuery] QueryOrgInput input);
        Task<List<OrgOutput>> GetOrgList([FromQuery] OrgInput input);
        Task<dynamic> GetOrgTree([FromQuery] OrgInput input);
        Task<dynamic> QueryOrgPageList([FromQuery] PageOrgInput input);
        Task UpdateOrg(UpdateOrgInput input);
        Task<List<long>> GetAllDataScopeIdList();
        Task<List<long>> GetUserDataScopeIdList();
    }
}