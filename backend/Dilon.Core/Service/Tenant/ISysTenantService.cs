using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysTenantService
    {
        Task AddTenant(AddTenantInput input);
        Task DeleteTenant(DeleteTenantInput input);
        Task<SysTenant> GetTenant([FromQuery] QueryTenantInput input);
        Task<dynamic> QueryTenantPageList([FromQuery] TenantInput input);
        Task UpdateTenant(UpdateTenantInput input);
    }
}