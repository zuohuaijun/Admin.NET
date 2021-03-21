using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysEmpExtOrgPosService
    {
        Task AddOrUpdate(long empId, List<EmpExtOrgPosOutput> extIdList);
        Task DeleteEmpExtInfoByUserId(long empId);
        Task<List<EmpExtOrgPosOutput>> GetEmpExtOrgPosList(long empId);
        Task<bool> HasExtOrgEmp(long orgId);
        Task<bool> HasExtPosEmp(long posId);
    }
}