using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysRoleMenuService
    {
        Task DeleteRoleMenuListByMenuIdList(List<long> menuIdList);

        Task DeleteRoleMenuListByRoleId(long roleId);

        Task<List<long>> GetRoleMenuIdList(List<long> roleIdList);

        Task GrantMenu(GrantRoleMenuInput input);
    }
}