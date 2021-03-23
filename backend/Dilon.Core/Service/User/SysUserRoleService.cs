using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 用户角色服务
    /// </summary>
    public class SysUserRoleService : ISysUserRoleService, ITransient
    {
        private readonly IRepository<SysUserRole> _sysUserRoleRep;  // 用户权限表仓储 

        private readonly ISysRoleService _sysRoleService;

        public SysUserRoleService(IRepository<SysUserRole> sysUserRoleRep, ISysRoleService sysRoleService)
        {
            _sysUserRoleRep = sysUserRoleRep;
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 获取用户的角色Id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetUserRoleIdList(long userId)
        {
            return await _sysUserRoleRep.DetachedEntities.Where(u => u.SysUserId == userId).Select(u => u.SysRoleId).ToListAsync();
        }

        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task GrantRole(UserInput input)
        {
            var userRoles = await _sysUserRoleRep.Where(u => u.SysUserId == long.Parse(input.Id)).ToListAsync();
            userRoles.ForEach(u =>
            {
                u.DeleteNow();
            });

            input.GrantRoleIdList.ForEach(u =>
            {
                new SysUserRole
                {
                    SysUserId = long.Parse(input.Id),
                    SysRoleId = u
                }.InsertNow();
            });
        }

        /// <summary>
        /// 获取用户所有角色的数据范围（组织机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetUserRoleDataScopeIdList(long userId, long orgId)
        {
            var roleIdList = await GetUserRoleIdList(userId);

            // 获取这些角色对应的数据范围
            if (roleIdList.Count > 0)
                return await _sysRoleService.GetUserDataScopeIdList(roleIdList, orgId);

            return roleIdList;
        }

        /// <summary>
        /// 根据角色Id删除对应的用户-角色表关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteUserRoleListByRoleId(long roleId)
        {
            var userRoles = await _sysUserRoleRep.Where(u => u.SysRoleId == roleId).ToListAsync();
            userRoles.ForEach(u =>
            {
                u.DeleteNow();
            });
        }

        /// <summary>
        /// 根据用户Id删除对应的用户-角色表关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserRoleListByUserId(long userId)
        {
            var userRoles = await _sysUserRoleRep.Where(u => u.SysUserId == userId).ToListAsync();
            userRoles.ForEach(u =>
            {
                u.DeleteNow();
            });
        }
    }
}
