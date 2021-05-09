using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 用户数据范围服务
    /// </summary>
    public class SysUserDataScopeService : ISysUserDataScopeService, ITransient
    {
        private readonly IRepository<SysUserDataScope> _sysUserDataScopeRep;  // 用户数据范围表仓储

        public SysUserDataScopeService(IRepository<SysUserDataScope> sysUserDataScopeRep)
        {
            _sysUserDataScopeRep = sysUserDataScopeRep;
        }

        /// <summary>
        /// 授权用户数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task GrantData(UpdateUserRoleDataInput input)
        {
            var dataScopes = await _sysUserDataScopeRep.Where(u => u.SysUserId == input.Id).ToListAsync();
            await _sysUserDataScopeRep.DeleteAsync(dataScopes);

            var userDataScopes = input.GrantOrgIdList.Select(u => new SysUserDataScope
            {
                SysUserId = input.Id,
                SysOrgId = u
            }).ToList();
            await _sysUserDataScopeRep.InsertAsync(userDataScopes);
        }

        /// <summary>
        /// 获取用户的数据范围Id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetUserDataScopeIdList(long userId)
        {
            return await _sysUserDataScopeRep.DetachedEntities
                                             .Where(u => u.SysUserId == userId)
                                             .Select(u => u.SysOrgId).ToListAsync();
        }

        /// <summary>
        /// 根据机构Id集合删除对应的用户-数据范围关联信息
        /// </summary>
        /// <param name="orgIdList"></param>
        /// <returns></returns>
        public async Task DeleteUserDataScopeListByOrgIdList(List<long> orgIdList)
        {
            var dataScopes = await _sysUserDataScopeRep.Where(u => orgIdList.Contains(u.SysOrgId)).ToListAsync();
            await _sysUserDataScopeRep.DeleteAsync(dataScopes);
        }

        /// <summary>
        /// 根据用户Id删除对应的用户-数据范围关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserDataScopeListByUserId(long userId)
        {
            var dataScopes = await _sysUserDataScopeRep.Where(u => u.SysUserId == userId).ToListAsync();
            await _sysUserDataScopeRep.DeleteAsync(dataScopes);
        }
    }
}