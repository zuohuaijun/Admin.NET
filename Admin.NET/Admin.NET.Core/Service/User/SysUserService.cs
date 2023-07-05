namespace Admin.NET.Core.Service;

/// <summary>
/// 系统用户服务
/// </summary>
[ApiDescriptionSettings(Order = 490)]
public class SysUserService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SysOrgService _sysOrgService;
    private readonly SysUserExtOrgService _sysUserExtOrgService;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysConfigService _sysConfigService;

    public SysUserService(UserManager userManager,
        SqlSugarRepository<SysUser> sysUserRep,
        SysOrgService sysOrgService,
        SysUserExtOrgService sysUserExtOrgService,
        SysUserRoleService sysUserRoleService,
        SysConfigService sysConfigService)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _sysOrgService = sysOrgService;
        _sysUserExtOrgService = sysUserExtOrgService;
        _sysUserRoleService = sysUserRoleService;
        _sysConfigService = sysConfigService;
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取用户分页列表")]
    public async Task<SqlSugarPagedList<SysUser>> Page(PageUserInput input)
    {
        var orgList = input.OrgId > 0 ? await _sysOrgService.GetChildIdListWithSelfById(input.OrgId) :
            _userManager.SuperAdmin ? null : await _sysOrgService.GetUserOrgIdList(); // 各管理员只能看到自己机构下的用户列表

        return await _sysUserRep.AsQueryable()
            .Where(u => u.AccountType != AccountTypeEnum.SuperAdmin)
            .WhereIF(orgList != null, u => orgList.Contains(u.OrgId))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Account), u => u.Account.Contains(input.Account))
            .WhereIF(!string.IsNullOrWhiteSpace(input.RealName), u => u.RealName.Contains(input.RealName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加用户")]
    public async Task AddUser(AddUserInput input)
    {
        var isExist = await _sysUserRep.AsQueryable().Filter(null, true).AnyAsync(u => u.Account == input.Account);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1003);

        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);

        var user = input.Adapt<SysUser>();
        user.Password = CryptogramUtil.Encrypt(password);
        var newUser = await _sysUserRep.AsInsertable(user).ExecuteReturnEntityAsync();
        input.Id = newUser.Id;
        await UpdateRoleAndExtOrg(input);
    }

    /// <summary>
    /// 更新角色和扩展机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task UpdateRoleAndExtOrg(AddUserInput input)
    {
        await GrantRole(new UserRoleInput { UserId = input.Id, RoleIdList = input.RoleIdList });

        await _sysUserExtOrgService.UpdateUserExtOrg(input.Id, input.ExtOrgIdList);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新用户")]
    public async Task UpdateUser(UpdateUserInput input)
    {
        var isExist = await _sysUserRep.AsQueryable().Filter(null, true).AnyAsync(u => u.Account == input.Account && u.Id != input.Id);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1003);

        await _sysUserRep.AsUpdateable(input.Adapt<SysUser>()).IgnoreColumns(true)
            .IgnoreColumns(u => new { u.AccountType, u.Password, u.Status }).ExecuteCommandAsync();

        await UpdateRoleAndExtOrg(input);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除用户")]
    public async Task DeleteUser(DeleteUserInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1002);
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1014);
        if (user.Id == _userManager.UserId)
            throw Oops.Oh(ErrorCodeEnum.D1001);

        await _sysUserRep.DeleteAsync(user);

        // 删除用户角色
        await _sysUserRoleService.DeleteUserRoleByUserId(input.Id);

        // 删除用户扩展机构
        await _sysUserExtOrgService.DeleteUserExtOrgByUserId(input.Id);
    }

    /// <summary>
    /// 查看用户基本信息
    /// </summary>
    /// <returns></returns>
    [DisplayName("查看用户基本信息")]
    public async Task<SysUser> GetBaseInfo()
    {
        return await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "BaseInfo"), HttpPost]
    [DisplayName("更新用户基本信息")]
    public async Task<int> UpdateBaseInfo(SysUser user)
    {
        return await _sysUserRep.AsUpdateable(user)
            .IgnoreColumns(u => new { u.CreateTime, u.Account, u.Password, u.AccountType, u.OrgId, u.PosId }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 设置用户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("设置用户状态")]
    public async Task<int> SetStatus(UserInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1015);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        user.Status = input.Status;
        return await _sysUserRep.AsUpdateable(user).UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("授权用户角色")]
    public async Task GrantRole(UserRoleInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.UserId);
        if (user.AccountType == AccountTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1022);

        await _sysUserRoleService.GrantUserRole(input);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("修改用户密码")]
    public async Task<int> ChangePwd(ChangePwdInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (user.Password != MD5Encryption.Encrypt(input.PasswordOld))
                throw Oops.Oh(ErrorCodeEnum.D1004);
        }
        else
        {
            if (CryptogramUtil.Decrypt(user.Password) != input.PasswordOld)
                throw Oops.Oh(ErrorCodeEnum.D1004);
        }

        user.Password = CryptogramUtil.Encrypt(input.PasswordNew);
        return await _sysUserRep.AsUpdateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("重置用户密码")]
    public async Task<string> ResetPwd(ResetPwdUserInput input)
    {
        var password = await _sysConfigService.GetConfigValue<string>(CommonConst.SysPassword);

        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        user.Password = CryptogramUtil.Encrypt(password);
        await _sysUserRep.AsUpdateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
        return password;
    }

    /// <summary>
    /// 获取用户拥有角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("获取用户拥有角色集合")]
    public async Task<List<long>> GetOwnRoleList(long userId)
    {
        return await _sysUserRoleService.GetUserRoleIdList(userId);
    }

    /// <summary>
    /// 获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [DisplayName("获取用户扩展机构集合")]
    public async Task<List<SysUserExtOrg>> GetOwnExtOrgList(long userId)
    {
        return await _sysUserExtOrgService.GetUserExtOrgList(userId);
    }
}