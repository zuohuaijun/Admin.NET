namespace Admin.NET.Core.Service;

/// <summary>
/// 系统角色服务
/// </summary>
[ApiDescriptionSettings(Name = "系统角色", Order = 198)]
public class SysRoleService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysRole> _sysRoleRep;
    private readonly IUserManager _userManager;
    private readonly ISysCacheService _sysCacheService;
    private readonly SysRoleOrgService _sysRoleOrgService;
    private readonly SysRoleMenuService _sysRoleMenuService;
    private readonly SysOrgService _sysOrgService;
    private readonly SysUserRoleService _sysUserRoleService;

    public SysRoleService(SqlSugarRepository<SysRole> sysRoleRep,
        IUserManager userManager,
        ISysCacheService sysCacheService,
        SysRoleOrgService sysRoleOrgService,
        SysRoleMenuService sysRoleMenuService,
        SysOrgService sysOrgService,
        SysUserRoleService sysUserRoleService)
    {
        _sysRoleRep = sysRoleRep;
        _userManager = userManager;
        _sysCacheService = sysCacheService;
        _sysRoleOrgService = sysRoleOrgService;
        _sysRoleMenuService = sysRoleMenuService;
        _sysOrgService = sysOrgService;
        _sysUserRoleService = sysUserRoleService;
    }

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysRole/pageList")]
    public async Task<SqlSugarPagedList<SysRole>> GetRolePageList([FromQuery] PageRoleInput input)
    {
        return await _sysRoleRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(input.Status > 0, u => u.Status == (StatusEnum)input.Status)
            .OrderBy(u => u.Order).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysRole/list")]
    public async Task<List<RoleOutput>> GetRoleList()
    {
        //// 若非超级管理员则只取拥有角色Id集合
        //var roleIdList = _userManager.SuperAdmin ? new List<long>() :
        //    await _sysUserRoleService.GetUserRoleIdList(_userManager.UserId);

        return await _sysRoleRep.AsQueryable()
            //.WhereIF(roleIdList.Count > 0, u => roleIdList.Contains(u.Id))
            .OrderBy(u => u.Order).Select<RoleOutput>().ToListAsync();
    }

    /// <summary>
    /// 增加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/add")]
    public async Task AddRole(AddRoleInput input)
    {
        var isExist = await _sysRoleRep.IsAnyAsync(u => u.Code == input.Code || u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var role = input.Adapt<SysRole>();
        await _sysRoleRep.InsertAsync(role);
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/update")]
    public async Task UpdateRole(UpdateRoleInput input)
    {
        var adminRole = await _sysRoleRep.GetFirstAsync(u => u.Id == input.Id);
        if (adminRole.Code == CommonConst.SysAdminRoleCode)
            throw Oops.Oh(ErrorCodeEnum.D1020);

        var isExist = await _sysRoleRep.IsAnyAsync(u => (u.Name == input.Name || u.Code == input.Code) && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var role = input.Adapt<SysRole>();
        await _sysRoleRep.AsUpdateable(role).IgnoreColumns(true).IgnoreColumns(u => new { u.DataScope }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/delete")]
    [SqlSugarUnitOfWork]
    public async Task DeleteRole(DeleteRoleInput input)
    {
        var sysRole = await _sysRoleRep.GetFirstAsync(u => u.Id == input.Id);
        if (sysRole.Code == CommonConst.SysAdminRoleCode)
            throw Oops.Oh(ErrorCodeEnum.D1019);

        await _sysRoleRep.DeleteAsync(sysRole);

        // 级联删除角色机构数据
        await _sysRoleOrgService.DeleteRoleOrgByRoleId(sysRole.Id);

        // 级联删除用户角色数据
        await _sysUserRoleService.DeleteUserRoleByRoleId(sysRole.Id);

        // 级联删除角色菜单数据
        await _sysRoleMenuService.DeleteRoleMenuByRoleId(sysRole.Id);
    }

    /// <summary>
    /// 授权角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/grantMenu")]
    public async Task GrantRoleMenu(RoleMenuInput input)
    {
        var role = await _sysRoleRep.GetFirstAsync(u => u.Id == input.Id);
        if (!_userManager.SuperAdmin && role.Code == CommonConst.SysAdminRoleCode)
            throw Oops.Oh(ErrorCodeEnum.D1021);

        await _sysRoleMenuService.GrantRoleMenu(input);
    }

    /// <summary>
    /// 授权角色数据范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/grantData")]
    public async Task GrantRoleDataScope(RoleOrgInput input)
    {
        // 删除所有用户机构缓存
        await _sysCacheService.DelByPatternAsync(CacheConst.KeyOrgIdList);

        var role = await _sysRoleRep.GetFirstAsync(u => u.Id == input.Id);
        var dataScope = input.DataScope;
        if (!_userManager.SuperAdmin)
        {
            // 非超级管理员没有全部数据范围权限
            if (dataScope == (int)DataScopeEnum.All)
                throw Oops.Oh(ErrorCodeEnum.D1016);

            // 若数据范围自定义，则判断授权数据范围是否有权限
            if (dataScope == (int)DataScopeEnum.Define)
            {
                var grantOrgIdList = input.OrgIdList;
                if (grantOrgIdList.Count > 0)
                {
                    var orgIdList = await _sysOrgService.GetUserOrgIdList();
                    if (orgIdList.Count < 1)
                        throw Oops.Oh(ErrorCodeEnum.D1016);
                    else if (!grantOrgIdList.All(u => orgIdList.Any(c => c == u)))
                        throw Oops.Oh(ErrorCodeEnum.D1016);
                }
            }
        }
        role.DataScope = (DataScopeEnum)dataScope;
        await _sysRoleRep.AsUpdateable(role).UpdateColumns(u => new { u.DataScope }).ExecuteCommandAsync();
        await _sysRoleOrgService.GrantRoleOrg(input);
    }

    /// <summary>
    /// 根据角色Id获取菜单树(前端区分父子节点)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysRole/ownMenu")]
    public async Task<List<SysMenu>> GetRoleOwnMenu([FromQuery] RoleInput input)
    {
        return await _sysRoleMenuService.GetRoleMenu(new List<long> { input.Id });
    }

    /// <summary>
    /// 根据角色Id获取机构Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysRole/ownOrg")]
    public async Task<List<long>> GetRoleOwnOrg([FromQuery] RoleInput input)
    {
        return await _sysRoleOrgService.GetRoleOrgIdList(new List<long> { input.Id });
    }

    /// <summary>
    /// 设置角色状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysRole/setStatus")]
    public async Task<int> SetRoleStatus(RoleInput input)
    {
        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        return await _sysRoleRep.AsUpdateable()
            .SetColumns(u => u.Status == (StatusEnum)input.Status)
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();
    }
}