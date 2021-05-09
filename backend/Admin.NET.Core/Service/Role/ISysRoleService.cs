using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysRoleService
    {
        Task AddRole(AddRoleInput input);

        Task DeleteRole(DeleteRoleInput input);

        Task<string> GetNameByRoleId(long roleId);

        Task<dynamic> GetRoleDropDown();

        Task<SysRole> GetRoleInfo([FromQuery] QueryRoleInput input);

        Task<dynamic> GetRoleList([FromQuery] RoleInput input);

        Task<List<long>> GetUserDataScopeIdList(List<long> roleIdList, long orgId);

        Task<List<RoleOutput>> GetUserRoleList(long userId);

        Task GrantData(GrantRoleDataInput input);

        Task GrantMenu(GrantRoleMenuInput input);

        Task<List<long>> OwnData([FromQuery] QueryRoleInput input);

        Task<List<long>> OwnMenu([FromQuery] QueryRoleInput input);

        Task<dynamic> QueryRolePageList([FromQuery] RolePageInput input);

        Task UpdateRole(UpdateRoleInput input);
    }
}