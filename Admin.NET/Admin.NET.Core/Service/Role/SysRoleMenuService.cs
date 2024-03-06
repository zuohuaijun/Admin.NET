// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统角色菜单服务
/// </summary>
public class SysRoleMenuService : ITransient
{
    private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenuRep;
    private readonly SysCacheService _sysCacheService;

    public SysRoleMenuService(SqlSugarRepository<SysRoleMenu> sysRoleMenuRep,
        SysCacheService sysCacheService)
    {
        _sysRoleMenuRep = sysRoleMenuRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 根据角色Id集合获取菜单Id集合
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    public async Task<List<long>> GetRoleMenuIdList(List<long> roleIdList)
    {
        return await _sysRoleMenuRep.AsQueryable()
            .Where(u => roleIdList.Contains(u.RoleId))
            .Select(u => u.MenuId).ToListAsync();
    }

    /// <summary>
    /// 授权角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task GrantRoleMenu(RoleMenuInput input)
    {
        await _sysRoleMenuRep.DeleteAsync(u => u.RoleId == input.Id);
        var menus = input.MenuIdList.Select(u => new SysRoleMenu
        {
            RoleId = input.Id,
            MenuId = u
        }).ToList();
        await _sysRoleMenuRep.InsertRangeAsync(menus);

        // 清除缓存
        // _sysCacheService.RemoveByPrefixKey(CacheConst.KeyUserMenu);
        _sysCacheService.RemoveByPrefixKey(CacheConst.KeyUserButton);
    }

    /// <summary>
    /// 根据菜单Id集合删除角色菜单
    /// </summary>
    /// <param name="menuIdList"></param>
    /// <returns></returns>
    public async Task DeleteRoleMenuByMenuIdList(List<long> menuIdList)
    {
        await _sysRoleMenuRep.DeleteAsync(u => menuIdList.Contains(u.MenuId));
    }

    /// <summary>
    /// 根据角色Id删除角色菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task DeleteRoleMenuByRoleId(long roleId)
    {
        await _sysRoleMenuRep.DeleteAsync(u => u.RoleId == roleId);
    }
}