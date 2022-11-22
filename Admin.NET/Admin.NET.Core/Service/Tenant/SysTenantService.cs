namespace Admin.NET.Core.Service;

/// <summary>
/// 系统租户管理服务
/// </summary>
[ApiDescriptionSettings(Order = 140)]
public class SysTenantService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysTenant> _sysTenantRep;
    private readonly SqlSugarRepository<SysOrg> _sysOrgRep;
    private readonly SqlSugarRepository<SysRole> _sysRoleRep;
    private readonly SqlSugarRepository<SysPos> _sysPosRep;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SqlSugarRepository<SysUserExtOrg> _sysUserExtOrgRep;
    private readonly SqlSugarRepository<SysRoleMenu> _sysRoleMenuRep;
    private readonly SqlSugarRepository<SysUserRole> _userRoleRep;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysRoleMenuService _sysRoleMenuService;
    private readonly SysConfigService _sysConfigService;
    private readonly SysCacheService _sysCacheService;

    public SysTenantService(SqlSugarRepository<SysTenant> sysTenantRep,
        SqlSugarRepository<SysOrg> sysOrgRep,
        SqlSugarRepository<SysRole> sysRoleRep,
        SqlSugarRepository<SysPos> sysPosRep,
        SqlSugarRepository<SysUser> sysUserRep,
        SqlSugarRepository<SysUserExtOrg> sysUserExtOrgRep,
        SqlSugarRepository<SysRoleMenu> sysRoleMenuRep,
        SqlSugarRepository<SysUserRole> userRoleRep,
        SysUserRoleService sysUserRoleService,
        SysRoleMenuService sysRoleMenuService,
        SysConfigService sysConfigService,
        SysCacheService sysCacheService)
    {
        _sysTenantRep = sysTenantRep;
        _sysOrgRep = sysOrgRep;
        _sysRoleRep = sysRoleRep;
        _sysPosRep = sysPosRep;
        _sysUserRep = sysUserRep;
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
    public async Task<SqlSugarPagedList<TenantOutput>> GetTenantPage([FromQuery] PageTenantInput input)
    {
        return await _sysTenantRep.AsQueryable()
            .LeftJoin<SysUser>((t, u) => t.UserId == u.Id)
            .LeftJoin<SysOrg>((t, u, o) => t.OrgId == o.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), (t, u) => u.Phone.Contains(input.Phone.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), (t, u, o) => o.Name.Contains(input.Name.Trim()))
            .Select((t, u, o) => new TenantOutput
            {
                Id = t.Id,
                OrgId = o.Id,
                Name = o.Name,
                UserId = u.Id,
                AdminName = u.Account,
                Phone = u.Phone,
                Email = u.Email,
                TenantType = t.TenantType,
                DbType = t.DbType,
                Connection = t.Connection,
                ConfigId = t.ConfigId,
                Order = t.Order,
                Remark = t.Remark,
                Status = t.Status,
            })
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取库隔离的租户列表
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<List<SysTenant>> GetTenantDbList()
    {
        return await _sysTenantRep.GetListAsync(u => u.TenantType == TenantTypeEnum.Db && u.Status == StatusEnum.Enable);
    }

    /// <summary>
    /// 增加租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/add")]
    public async Task AddTenant(AddTenantInput input)
    {
        var isExist = await _sysOrgRep.IsAnyAsync(u => u.Name == input.Name);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1300);

        isExist = await _sysUserRep.AsQueryable().Filter(null, true).AnyAsync(u => u.Account == input.AdminName);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1301);

        // ID隔离时设置与主库一致
        if (input.TenantType == TenantTypeEnum.Id)
            input.DbType = _sysTenantRep.AsSugarClient().CurrentConnectionConfig.DbType;

        var tenant = input.Adapt<TenantOutput>();
        await _sysTenantRep.InsertAsync(tenant);
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
        var tenant = await _sysTenantRep.GetFirstAsync(u => u.Id == input.Id);
        if (tenant.ConfigId == SqlSugarConst.ConfigId)
            throw Oops.Oh(ErrorCodeEnum.Z1001);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        tenant.Status = input.Status;
        return await _sysTenantRep.AsUpdateable(tenant).UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增租户初始化
    /// </summary>
    /// <param name="newTenant"></param>
    private async Task InitNewTenant(TenantOutput newTenant)
    {
        var tenantId = newTenant.Id;
        var tenantName = newTenant.Name;

        // 初始化机构
        var newOrg = new SysOrg
        {
            TenantId = tenantId,
            Pid = 0,
            Name = tenantName,
            Code = tenantName,
            Remark = tenantName,
        };
        await _sysOrgRep.InsertAsync(newOrg);

        // 初始化角色
        var newRole = new SysRole
        {
            TenantId = tenantId,
            Name = "租管-" + tenantName,
            Code = CommonConst.SysAdminRole,
            DataScope = DataScopeEnum.All,
            Remark = tenantName
        };
        await _sysRoleRep.InsertAsync(newRole);

        // 初始化职位
        var newPos = new SysPos
        {
            TenantId = tenantId,
            Name = "租管-" + tenantName,
            Code = tenantName,
            Remark = tenantName,
        };
        await _sysPosRep.InsertAsync(newPos);

        // 初始化系统账号
        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);
        var newUser = new SysUser
        {
            TenantId = tenantId,
            Account = newTenant.AdminName,
            Password = MD5Encryption.Encrypt(password),
            NickName = newTenant.AdminName,
            Email = newTenant.Email,
            Phone = newTenant.Phone,
            AccountType = AccountTypeEnum.Admin,
            OrgId = newOrg.Id,
            PosId = newPos.Id,
            Birthday = DateTime.Parse("1986-06-28"),
            RealName = "租管",
            Remark = "租管" + tenantName,
        };
        await _sysUserRep.InsertAsync(newUser);

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

        await _sysTenantRep.DeleteAsync(u => u.Id == input.Id);

        await UpdateTenantCache();

        // 删除与租户相关的表数据
        var users = await _sysUserRep.AsQueryable().Filter(null, true).Where(u => u.TenantId == input.Id).ToListAsync();
        var userIds = users.Select(u => u.Id).ToList();
        await _sysUserRep.AsDeleteable().Where(u => userIds.Contains(u.Id)).ExecuteCommandAsync();

        await _userRoleRep.AsDeleteable().Where(u => userIds.Contains(u.UserId)).ExecuteCommandAsync();

        await _sysUserExtOrgRep.AsDeleteable().Where(u => userIds.Contains(u.UserId)).ExecuteCommandAsync();

        await _sysRoleRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();

        var roleIds = await _sysRoleRep.AsQueryable().Filter(null, true)
            .Where(u => u.TenantId == input.Id).Select(u => u.Id).ToListAsync();
        await _sysRoleMenuRep.AsDeleteable().Where(u => roleIds.Contains(u.RoleId)).ExecuteCommandAsync();

        await _sysOrgRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();

        await _sysPosRep.AsDeleteable().Where(u => u.TenantId == input.Id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTenant/update")]
    public async Task UpdateTenant(UpdateTenantInput input)
    {
        await _sysTenantRep.AsUpdateable(input.Adapt<TenantOutput>()).IgnoreColumns(true).ExecuteCommandAsync();

        // 更新系统机构
        await _sysOrgRep.UpdateSetColumnsTrueAsync(u => new SysOrg() { Name = input.Name }, u => u.Id == input.OrgId);

        // 更新系统用户
        await _sysUserRep.UpdateSetColumnsTrueAsync(u => new SysUser() { Account = input.AdminName, Phone = input.Phone, Email = input.Email }, u => u.Id == input.UserId);

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
        var tenantAdminUser = await _sysUserRep.GetFirstAsync(u => u.TenantId == input.Id && u.AccountType == AccountTypeEnum.Admin);
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
        var roleIds = await _sysUserRoleService.GetUserRoleIdList(input.UserId);
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
        var roleIds = await _sysUserRoleService.GetUserRoleIdList(input.UserId);
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
        var encryptPassword = MD5Encryption.Encrypt(password);
        await _sysUserRep.UpdateSetColumnsTrueAsync(u => new SysUser() { Password = password }, u => u.Id == input.UserId);
    }

    /// <summary>
    /// 缓存所有租户
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task UpdateTenantCache()
    {
        _sysCacheService.Remove(CacheConst.KeyTenant);

        var tenantList = await _sysTenantRep.GetListAsync();
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
        var tenant = await _sysTenantRep.GetFirstAsync(u => u.Id == input.Id);
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