namespace Admin.NET.Core.Service;

/// <summary>
/// 系统租户管理服务
/// </summary>
[ApiDescriptionSettings(Order = 140)]
public class SysTenantService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysTenant> _tenantRep;
    private readonly SqlSugarRepository<SysOrg> _orgRep;
    private readonly SqlSugarRepository<SysRole> _roleRep;
    private readonly SqlSugarRepository<SysPos> _posRep;
    private readonly SqlSugarRepository<SysUser> _userRep;
    private readonly SqlSugarRepository<SysUserExtOrg> _sysUserExtOrgRep;
    private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenuRep;
    private readonly SqlSugarRepository<SysUserRole> _userRoleRep;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysRoleMenuService _sysRoleMenuService;
    private readonly SysConfigService _sysConfigService;
    private readonly SysCacheService _sysCacheService;

    public SysTenantService(SqlSugarRepository<SysTenant> tenantRep,
        SqlSugarRepository<SysOrg> orgRep,
        SqlSugarRepository<SysRole> roleRep,
        SqlSugarRepository<SysPos> posRep,
        SqlSugarRepository<SysUser> userRep,
        SqlSugarRepository<SysUserExtOrg> sysUserExtOrgRep,
        SqlSugarRepository<SysRoleMenu> sysRoleMenuRep,
        SqlSugarRepository<SysUserRole> userRoleRep,
        SysUserRoleService sysUserRoleService,
        SysRoleMenuService sysRoleMenuService,
        SysConfigService sysConfigService,
        SysCacheService sysCacheService)
    {
        _tenantRep = tenantRep;
        _orgRep = orgRep;
        _roleRep = roleRep;
        _posRep = posRep;
        _userRep = userRep;
        _sysUserExtOrgRep = sysUserExtOrgRep;
        _sysRoleMenuRep = sysRoleMenuRep;
        _userRoleRep = userRoleRep;
        _sysUserRoleService = sysUserRoleService;
        _sysRoleMenuService = sysRoleMenuService;
        _sysConfigService = sysConfigService;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取租户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysTenant/page")]
    public async Task<SqlSugarPagedList<SysTenant>> GetTenantPage([FromQuery] PageTenantInput input)
    {
        return await _tenantRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取库隔离的租户列表
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<List<SysTenant>> GetTenantDbList()
    {
        return await _tenantRep.GetListAsync(u => u.TenantType == TenantTypeEnum.Db && u.Status == StatusEnum.Enable);
    }

    /// <summary>
    /// 增加租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/add")]
    public async Task AddTenant(AddTenantInput input)
    {
        var isExist = await _tenantRep.IsAnyAsync(u => u.Name == input.Name);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1300);

        isExist = await _userRep.AsQueryable().Filter(null, true).AnyAsync(u => u.Account == input.AdminName);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1301);

        // ID隔离时设置与主库一致
        if (input.TenantType == TenantTypeEnum.Id)
            input.DbType = _tenantRep.AsSugarClient().CurrentConnectionConfig.DbType;

        var tenant = input.Adapt<SysTenant>();
        await _tenantRep.InsertAsync(tenant);
        await UpdateTenantCache();

        await InitNewTenant(tenant);
    }

    /// <summary>
    /// 设置租户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/setStatus")]
    public async Task<int> SetTenantStatus(TenantInput input)
    {
        var tenant = await _tenantRep.GetFirstAsync(u => u.Id == input.Id);
        if (tenant.ConfigId == SqlSugarConst.ConfigId)
            throw Oops.Oh(ErrorCodeEnum.Z1001);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        tenant.Status = input.Status;
        return await _tenantRep.AsUpdateable(tenant).UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增租户初始化（Id隔离）
    /// </summary>
    /// <param name="newTenant"></param>
    private async Task InitNewTenant(SysTenant newTenant)
    {
        long tenantId = newTenant.Id;
        string admin = newTenant.AdminName;
        string companyName = newTenant.Name;
        // 初始化租户（组织结构）
        var newOrg = new SysOrg
        {
            TenantId = tenantId,
            Pid = 0,
            Name = companyName,
            Code = companyName,
            Remark = companyName,
        };
        await _orgRep.InsertAsync(newOrg);

        // 初始化角色
        var newRole = new SysRole
        {
            TenantId = tenantId,
            Code = CommonConst.SysAdminRole,
            Name = "租户管理员-" + companyName,
            DataScope = DataScopeEnum.All,
            Remark = companyName
        };
        await _roleRep.InsertAsync(newRole);

        var newPos = new SysPos
        {
            Name = "租户管理员",
            Code = "adminTenant",
            TenantId = tenantId,
            Remark = companyName,
        };
        await _posRep.InsertAsync(newPos);

        // 初始化租户管理员
        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);
        var newUser = new SysUser
        {
            TenantId = tenantId,
            Account = admin,
            Password = MD5Encryption.Encrypt(password),
            NickName = newTenant.AdminName,
            Email = newTenant.Email,
            Phone = newTenant.Phone,
            AccountType = AccountTypeEnum.Admin,
            OrgId = newOrg.Id,
            PosId = newPos.Id,
            Birthday = DateTime.Parse("1986-06-28"),
            RealName = "租户管理员",
            Remark = "租户管理员" + companyName,
        };
        await _userRep.InsertAsync(newUser);

        var newUserRole = new SysUserRole
        {
            RoleId = newRole.Id,
            UserId = newUser.Id
        };
        await _userRoleRep.InsertAsync(newUserRole);

        // 默认租户管理员角色菜单集合
        var menuIdList = new List<long> { 252885263002100,252885263002110,252885263002111,
            252885263005200,252885263005210,252885263005211,252885263005212,252885263005213,252885263005214,252885263005215,252885263005216,252885263005217,252885263005218,252885263005219,252885263005220,
            252885263005230,252885263005231,252885263005232,252885263005233,252885263005234,252885263005235,252885263005236,252885263005237,
            252885263005240,252885263005241,252885263005242,252885263005243,252885263005244,
            252885263005250,252885263005251,252885263005252,252885263005253,252885263005254,
            252885263005260,252885263005261,252885263005262,252885263005263,
            252885263005270,252885263005271,252885263005272,252885263005273,252885263005274,252885263005275,252885263005276
        };
        await _sysRoleMenuService.GrantRoleMenu(new RoleMenuInput() { Id = newRole.Id, MenuIdList = menuIdList });
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/delete")]
    public async Task DeleteTenant(DeleteTenantInput input)
    {
        // 禁止删除默认租户
        if (input.Id.ToString() == SqlSugarConst.ConfigId)
            throw Oops.Oh(ErrorCodeEnum.D1023);

        var entity = await _tenantRep.GetFirstAsync(u => u.Id == input.Id);
        await _tenantRep.DeleteAsync(entity);
        await UpdateTenantCache();

        // 删除与租户相关的表数据
        var users = await _userRep.AsQueryable().Filter(null, true).Where(u => u.TenantId == input.Id).ToListAsync();
        var userIds = users.Select(u => u.Id).ToList();
        await _userRep.AsDeleteable().Where(u => userIds.Contains(u.Id)).ExecuteCommandAsync();

        await _userRoleRep.AsDeleteable().Where(u => userIds.Contains(u.UserId)).ExecuteCommandAsync();

        await _sysUserExtOrgRep.AsDeleteable().Where(u => userIds.Contains(u.UserId)).ExecuteCommandAsync();

        await _roleRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();

        var roleIds = await _roleRep.AsQueryable().Filter(null, true)
            .Where(u => u.TenantId == input.Id).Select(u => u.Id).ToListAsync();
        await _sysRoleMenuRep.AsDeleteable().Where(u => roleIds.Contains(u.RoleId)).ExecuteCommandAsync();

        await _orgRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();

        await _posRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/update")]
    public async Task UpdateTenant(UpdateTenantInput input)
    {
        var entity = input.Adapt<SysTenant>();
        await _tenantRep.Context.Updateable(entity).IgnoreColumns(true).ExecuteCommandAsync();

        var tenantAdminUser = await GetTenantAdminUser(input.Id);
        if (tenantAdminUser == null) return;
        tenantAdminUser.Account = entity.AdminName;
        await _userRep.Context.Updateable(tenantAdminUser).UpdateColumns(u => new { u.Account }).ExecuteCommandAsync();
        await UpdateTenantCache();
    }

    /// <summary>
    /// 授权租户管理员角色菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/grantMenu")]
    public async Task GrantMenu(RoleMenuInput input)
    {
        var tenantAdminUser = await GetTenantAdminUser(input.Id);
        if (tenantAdminUser == null) return;
        var roleIds = await _sysUserRoleService.GetUserRoleIdList(tenantAdminUser.Id);
        input.Id = roleIds[0]; // 重置租户管理员角色Id
        await _sysRoleMenuService.GrantRoleMenu(input);
    }

    /// <summary>
    /// 获取租户管理员角色拥有菜单树
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysTenant/ownMenuTree")]
    public async Task<List<SysMenu>> OwnMenuTree([FromQuery] TenantInput input)
    {
        var tenantAdminUser = await GetTenantAdminUser(input.Id);
        if (tenantAdminUser == null) return new List<SysMenu>();
        var roleIds = await _sysUserRoleService.GetUserRoleIdList(tenantAdminUser.Id);
        return await _sysRoleMenuService.GetRoleMenuTree(new List<long> { roleIds[0] });
    }

    /// <summary>
    /// 获取租户管理员角色拥有菜单树
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysTenant/ownMenuList")]
    public async Task<List<long>> OwnMenuList([FromQuery] TenantInput input)
    {
        var tenantAdminUser = await GetTenantAdminUser(input.Id);
        if (tenantAdminUser == null) return new List<long>();
        var roleIds = await _sysUserRoleService.GetUserRoleIdList(tenantAdminUser.Id);
        return await _sysRoleMenuService.GetRoleMenuList(new List<long> { roleIds[0] });
    }

    /// <summary>
    /// 重置租户管理员密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/resetPwd")]
    public async Task ResetTenantPwd(TenantInput input)
    {
        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);

        var tenantAdminUser = await GetTenantAdminUser(input.Id);
        tenantAdminUser.Password = MD5Encryption.Encrypt(password);
        await _userRep.UpdateAsync(tenantAdminUser);
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<SysTenant> GetTenant(long tenantId)
    {
        return await _tenantRep.GetFirstAsync(u => u.Id == tenantId);
    }

    /// <summary>
    /// 获取租户管理员用户
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    private async Task<SysUser> GetTenantAdminUser(long tenantId)
    {
        return await _userRep.GetFirstAsync(u => u.TenantId == tenantId && u.AccountType == AccountTypeEnum.Admin);
    }

    /// <summary>
    /// 缓存所有租户
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task UpdateTenantCache()
    {
        _sysCacheService.Remove(CacheConst.KeyTenant);

        var tenantList = await _tenantRep.GetListAsync();
        var defautTenant = tenantList.FirstOrDefault(u => u.Id.ToString() == SqlSugarConst.ConfigId);
        foreach (var tenant in tenantList)
        {
            if (tenant.Id.ToString() == SqlSugarConst.ConfigId) continue;

            // Id模式隔离的租户数据库与主租户一致
            if (tenant.TenantType == TenantTypeEnum.Id)
            {
                tenant.ConfigId = tenant.Id.ToString();
                tenant.DbType = defautTenant.DbType;
                tenant.Connection = defautTenant.Connection;
            }
        }

        _sysCacheService.Set(CacheConst.KeyTenant, tenantList);
    }

    /// <summary>
    /// 创建租户数据库
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/createDb")]
    public async Task CreateTenantDb(TenantInput input)
    {
        var tenant = await _tenantRep.GetFirstAsync(u => u.Id == input.Id);
        if (tenant == null) return;

        if (tenant.DbType == SqlSugar.DbType.Oracle)
            throw Oops.Oh(ErrorCodeEnum.Z1002);

        var config = new DbConnectionConfig
        {
            EnableInitDb = true,
            EnableDiffLog = false,
            DbType = tenant.DbType,
            ConfigId = tenant.Id.ToString(),
            ConnectionString = tenant.Connection,
            IsAutoCloseConnection = true,
        };
        SqlSugarSetup.InitTenantDatabase(App.GetRequiredService<ISqlSugarClient>().AsTenant(), config);
    }
}