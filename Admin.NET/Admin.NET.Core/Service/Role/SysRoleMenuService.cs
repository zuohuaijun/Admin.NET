namespace Admin.NET.Core.Service;

/// <summary>
/// 系统角色菜单服务
/// </summary>
public class SysRoleMenuService : ITransient
{
    private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenuRep;
    private readonly ISysCacheService _sysCacheService;

    public SysRoleMenuService(SqlSugarRepository<SysRoleMenu> sysRoleMenuRep,
        ISysCacheService sysCacheService)
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
    /// 根据角色Id集合获取菜单树
    /// </summary>
    /// <param name="roleIdList"></param>
    /// <returns></returns>
    public async Task<List<SysMenu>> GetRoleMenu(List<long> roleIdList)
    {
        var menuIdList = await _sysRoleMenuRep.AsQueryable()
            .Where(u => roleIdList.Contains(u.RoleId))
            .Select(u => u.MenuId).ToListAsync();

        return await _sysRoleMenuRep.ChangeRepository<SqlSugarRepository<SysMenu>>().AsQueryable()
            .Where(u => menuIdList.Contains(u.Id))
            .ToTreeAsync(u => u.Children, u => u.Pid, 0);
    }

    /// <summary>
    /// 授权角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [SqlSugarUnitOfWork]
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
        await _sysCacheService.DelByPatternAsync(CacheConst.KeyMenu);
        await _sysCacheService.DelByPatternAsync(CacheConst.KeyPermission);
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