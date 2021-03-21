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
    /// 角色菜单
    /// </summary>
    public class SysRoleMenuService : ISysRoleMenuService, ITransient
    {
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRep;  // 角色菜单表仓储   

        public SysRoleMenuService(IRepository<SysRoleMenu> sysRoleMenuRep)
        {
            _sysRoleMenuRep = sysRoleMenuRep;
        }

        /// <summary>
        /// 获取角色的菜单Id集合
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        public async Task<List<long>> GetRoleMenuIdList(List<long> roleIdList)
        {
            return await _sysRoleMenuRep.DetachedEntities
                                        .Where(u => roleIdList.Contains(u.SysRoleId))
                                        .Select(u => u.SysMenuId).ToListAsync();
        }

        /// <summary>
        /// 授权角色菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task GrantMenu(GrantRoleMenuInput input)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => u.SysRoleId == input.Id).ToListAsync();
            roleMenus.ForEach(u =>
            {
                u.DeleteNowAsync();
            });

            input.GrantMenuIdList.ForEach(u =>
            {
                new SysRoleMenu
                {
                    SysRoleId = input.Id,
                    SysMenuId = u
                }.InsertNowAsync();
            });
        }

        /// <summary>
        /// 根据菜单Id集合删除对应的角色-菜单表信息
        /// </summary>
        /// <param name="menuIdList"></param>
        /// <returns></returns>
        public async Task DeleteRoleMenuListByMenuIdList(List<long> menuIdList)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => menuIdList.Contains(u.SysMenuId)).ToListAsync();
            roleMenus.ForEach(u =>
            {
                u.DeleteNowAsync();
            });
        }

        /// <summary>
        /// 根据角色Id删除对应的角色-菜单表关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteRoleMenuListByRoleId(long roleId)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => u.SysRoleId == roleId).ToListAsync();
            roleMenus.ForEach(u =>
            {
                u.DeleteNowAsync();
            });
        }
    }
}
