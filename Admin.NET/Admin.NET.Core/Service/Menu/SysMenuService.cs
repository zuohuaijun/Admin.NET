namespace Admin.NET.Core.Service;

/// <summary>
/// 系统菜单服务
/// </summary>
[ApiDescriptionSettings(Order = 195)]
public class SysMenuService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysMenu> _sysMenuRep;
    private readonly IUserManager _userManager;
    private readonly SysRoleMenuService _sysRoleMenuService;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysCacheService _sysCacheService;

    public SysMenuService(SqlSugarRepository<SysMenu> sysMenuRep,
        IUserManager userManager,
        SysRoleMenuService sysRoleMenuService,
        SysUserRoleService sysUserRoleService,
        SysCacheService sysCacheService)
    {
        _sysMenuRep = sysMenuRep;
        _userManager = userManager;
        _sysRoleMenuService = sysRoleMenuService;
        _sysUserRoleService = sysUserRoleService;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取登录菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet("/getMenuList")]
    public async Task<List<MenuOutput>> GetLoginMenuTree()
    {
        if (_userManager.SuperAdmin)
        {
            var menuList = await _sysMenuRep.AsQueryable()
                .Where(u => u.Type != MenuTypeEnum.Btn)
                .OrderBy(u => u.OrderNo).ToTreeAsync(u => u.Children, u => u.Pid, 0);
            return menuList.Adapt<List<MenuOutput>>();
        }
        else
        {
            var menuIdList = await GetMenuIdList();
            var menuList = await _sysMenuRep.AsQueryable()
                .Where(u => u.Type != MenuTypeEnum.Btn)
                .Where(u => menuIdList.Contains(u.Id))
                .OrderBy(u => u.OrderNo).ToTreeAsync(u => u.Children, u => u.Pid, 0);
            return menuList.Adapt<List<MenuOutput>>();
        }
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysMenu/list")]
    public async Task<List<SysMenu>> GetMenuList([FromQuery] MenuInput input)
    {
        var menuIdList = new List<long>();
        if (!_userManager.SuperAdmin)
            menuIdList = await GetMenuIdList();

        if (!string.IsNullOrWhiteSpace(input.Title) || input.Type > 0)
        {
            return await _sysMenuRep.AsQueryable()
                .WhereIF(input.Type > 0, u => u.Type == (MenuTypeEnum)input.Type)
                .WhereIF(menuIdList.Count > 1, u => menuIdList.Contains(u.Id))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title))
                .OrderBy(u => u.OrderNo).ToListAsync();
        }

        return await _sysMenuRep.AsQueryable()
            .WhereIF(menuIdList.Count > 1, u => menuIdList.Contains(u.Id))
            .OrderBy(u => u.OrderNo)
            .ToTreeAsync(u => u.Children, u => u.Pid, 0);
    }

    /// <summary>
    /// 增加菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysMenu/add")]
    public async Task AddMenu(AddMenuInput input)
    {
        var isExist = input.Type != (int)MenuTypeEnum.Btn
            ? await _sysMenuRep.IsAnyAsync(u => u.Name == input.Name)
            : await _sysMenuRep.IsAnyAsync(u => u.Permission == input.Permission);

        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D4000);

        // 校验菜单参数
        CheckMenuParam(input.Adapt<MenuInput>());

        await _sysMenuRep.InsertAsync(input.Adapt<SysMenu>());

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysMenu/update"),]
    public async Task UpdateMenu(UpdateMenuInput input)
    {
        var isExist = input.Type != (int)MenuTypeEnum.Btn
            ? await _sysMenuRep.IsAnyAsync(u => u.Name == input.Name && u.Id != input.Id)
            : await _sysMenuRep.IsAnyAsync(u => u.Permission == input.Permission && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D4000);

        // 校验菜单参数
        CheckMenuParam(input.Adapt<MenuInput>());

        await _sysMenuRep.AsUpdateable(input.Adapt<SysMenu>()).IgnoreColumns(true).ExecuteCommandAsync();

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysMenu/delete")]
    [UnitOfWork]
    public async Task DeleteMenu(DeleteMenuInput input)
    {
        var menuTreeList = await _sysMenuRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id);
        var menuIdList = menuTreeList.Select(u => u.Id).ToList();

        await _sysMenuRep.DeleteAsync(u => menuIdList.Contains(u.Id));

        // 级联删除角色菜单数据
        await _sysRoleMenuService.DeleteRoleMenuByMenuIdList(menuIdList);

        // 清除缓存
        DeleteMenuCache();
    }

    /// <summary>
    /// 增加和编辑时检查菜单数据
    /// </summary>
    /// <param name="input"></param>
    private static void CheckMenuParam(MenuInput input)
    {
        var type = input.Type;
        //var component = input.Component;
        var permission = input.Permission;
        //if (type == (int)MenuTypeEnum.Dir || type == (int)MenuTypeEnum.Menu)
        //{
        //    if (string.IsNullOrEmpty(component))
        //        throw Oops.Oh(ErrorCodeEnum.D4001);
        //}
        //else
        if (type == (int)MenuTypeEnum.Btn)
        {
            if (string.IsNullOrEmpty(permission))
                throw Oops.Oh(ErrorCodeEnum.D4003);
            if (!permission.Contains(':'))
                throw Oops.Oh(ErrorCodeEnum.D4004);
        }
    }

    /// <summary>
    /// 获取按钮权限列表(登录)
    /// </summary>
    /// <returns></returns>
    [HttpGet("getPermCode")]
    public async Task<List<string>> GetPermCodeList()
    {
        var userId = _userManager.UserId;
        var permissions = _sysCacheService.GetPermission(userId); // 先从缓存里面读取
        if (permissions == null || permissions.Count == 0)
        {
            if (_userManager.SuperAdmin)
            {
                permissions = await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type == MenuTypeEnum.Btn)
                    .Select(u => u.Permission).ToListAsync();
            }
            else
            {
                var menuIdList = await GetMenuIdList();
                permissions = await _sysMenuRep.AsQueryable()
                    .Where(u => u.Type == MenuTypeEnum.Btn)
                    .Where(u => menuIdList.Contains(u.Id))
                    .Select(u => u.Permission).ToListAsync();
            }
            _sysCacheService.SetPermission(userId, permissions); // 缓存结果
        }
        return permissions;
    }

    /// <summary>
    /// 获取所有按钮权限集合
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<List<string>> GetAllPermCodeList()
    {
        var permissions = _sysCacheService.GetPermission(0); // 先从缓存里面读取
        if (permissions == null || permissions.Count == 0)
        {
            permissions = await _sysMenuRep.AsQueryable()
                .Where(u => u.Type == MenuTypeEnum.Btn)
                .Select(u => u.Permission).ToListAsync();
            _sysCacheService.SetPermission(0, permissions); // 缓存结果
        }
        return permissions;
    }

    /// <summary>
    /// 清除菜单和按钮缓存
    /// </summary>
    private void DeleteMenuCache()
    {
        _sysCacheService.RemoveByPrefixKey(CacheConst.KeyMenu);
        _sysCacheService.RemoveByPrefixKey(CacheConst.KeyPermission);
    }

    /// <summary>
    /// 获取当前用户菜单Id集合
    /// </summary>
    /// <returns></returns>
    private async Task<List<long>> GetMenuIdList()
    {
        var roleIdList = await _sysUserRoleService.GetUserRoleIdList(_userManager.UserId);
        return await _sysRoleMenuService.GetRoleMenuIdList(roleIdList);
    }
}