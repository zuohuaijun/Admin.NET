using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 角色数据范围服务
    /// </summary>
    public class SysRoleDataScopeService : ISysRoleDataScopeService, ITransient
    {
        private readonly IRepository<SysRoleDataScope> _sysRoleDataScopeRep;  // 角色数据范围表仓储

        public SysRoleDataScopeService(IRepository<SysRoleDataScope> sysRoleDataScopeRep)
        {
            _sysRoleDataScopeRep = sysRoleDataScopeRep;
        }

        /// <summary>
        /// 授权角色数据范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task GrantDataScope(GrantRoleDataInput input)
        {
            var dataScopes = await _sysRoleDataScopeRep.DetachedEntities.Where(u => u.SysRoleId == input.Id).ToListAsync();
            await _sysRoleDataScopeRep.DeleteAsync(dataScopes);

            var roleDataScopes = input.GrantOrgIdList.Select(u => new SysRoleDataScope
            {
                SysRoleId = input.Id,
                SysOrgId = u
            }).ToList();
            await _sysRoleDataScopeRep.InsertAsync(roleDataScopes);
        }

        /// <summary>
        /// 根据角色Id集合获取角色数据范围集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        public async Task<List<long>> GetRoleDataScopeIdList(List<long> roleIdList)
        {
            return await _sysRoleDataScopeRep.DetachedEntities
                                             .Where(u => roleIdList.Contains(u.SysRoleId))
                                             .Select(u => u.SysOrgId).ToListAsync();
        }

        /// <summary>
        /// 根据机构Id集合删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="orgIdList"></param>
        /// <returns></returns>
        public async Task DeleteRoleDataScopeListByOrgIdList(List<long> orgIdList)
        {
            var dataScopes = await _sysRoleDataScopeRep.DetachedEntities.Where(u => orgIdList.Contains(u.SysOrgId)).ToListAsync();
            await _sysRoleDataScopeRep.DeleteAsync(dataScopes);
        }

        /// <summary>
        /// 根据角色Id删除对应的角色-数据范围关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteRoleDataScopeListByRoleId(long roleId)
        {
            var dataScopes = await _sysRoleDataScopeRep.DetachedEntities.Where(u => u.SysRoleId == roleId).ToListAsync();
            await _sysRoleDataScopeRep.DeleteAsync(dataScopes);
        }
    }
}