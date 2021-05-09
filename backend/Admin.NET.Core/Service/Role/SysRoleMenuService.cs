using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class SysRoleMenuService : ISysRoleMenuService, ITransient
    {
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRep;  // 角色菜单表仓储
        private readonly ISysCacheService _sysCacheService;

        public SysRoleMenuService(IRepository<SysRoleMenu> sysRoleMenuRep, ISysCacheService sysCacheService)
        {
            _sysRoleMenuRep = sysRoleMenuRep;
            _sysCacheService = sysCacheService;
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
        [UnitOfWork]
        public async Task GrantMenu(GrantRoleMenuInput input)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => u.SysRoleId == input.Id).ToListAsync();
            await _sysRoleMenuRep.DeleteAsync(roleMenus);

            var menus = input.GrantMenuIdList.Select(u => new SysRoleMenu
            {
                SysRoleId = input.Id,
                SysMenuId = u
            }).ToList();
            await _sysRoleMenuRep.InsertAsync(menus);

            // 清除缓存
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_MENU);
            await _sysCacheService.DelByPatternAsync(CommonConst.CACHE_KEY_PERMISSION);
        }

        /// <summary>
        /// 根据菜单Id集合删除对应的角色-菜单表信息
        /// </summary>
        /// <param name="menuIdList"></param>
        /// <returns></returns>
        public async Task DeleteRoleMenuListByMenuIdList(List<long> menuIdList)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => menuIdList.Contains(u.SysMenuId)).ToListAsync();
            await _sysRoleMenuRep.DeleteAsync(roleMenus);
        }

        /// <summary>
        /// 根据角色Id删除对应的角色-菜单表关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteRoleMenuListByRoleId(long roleId)
        {
            var roleMenus = await _sysRoleMenuRep.DetachedEntities.Where(u => u.SysRoleId == roleId).ToListAsync();
            await _sysRoleMenuRep.DeleteAsync(roleMenus);
        }
    }
}