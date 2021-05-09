using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysOrgService
    {
        Task AddOrg(OrgAddInput input);

        Task DeleteOrg(DeleteOrgInput input);

        Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId);

        Task<SysOrg> GetOrg([FromQuery] QueryOrgInput input);

        Task<List<OrgOutput>> GetOrgList([FromQuery] OrgListInput input);

        Task<dynamic> GetOrgTree();

        Task<dynamic> QueryOrgPageList([FromQuery] OrgPageInput input);

        Task UpdateOrg(UpdateOrgInput input);

        Task<List<long>> GetAllDataScopeIdList();

        Task<List<long>> GetUserDataScopeIdList();
    }
}