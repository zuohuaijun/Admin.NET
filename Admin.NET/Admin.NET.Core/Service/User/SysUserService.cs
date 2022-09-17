namespace Admin.NET.Core.Service;

/// <summary>
/// 系统用户服务
/// </summary>
[ApiDescriptionSettings(Order = 199)]
public class SysUserService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly IUserManager _userManager;
    private readonly SysCacheService _sysCacheService;
    private readonly SysOrgService _sysOrgService;
    private readonly SysUserOrgService _sysUserOrgService;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysUserExtOrgPosService _sysUserExtOrgPosService;

    public SysUserService(SqlSugarRepository<SysUser> sysUserRep,
        IUserManager userManager,
        SysCacheService sysCacheService,
        SysOrgService sysOrgService,
        SysUserOrgService sysUserOrgService,
        SysUserRoleService sysUserRoleService,
        SysUserExtOrgPosService sysUserExtOrgPosService)
    {
        _sysUserRep = sysUserRep;
        _userManager = userManager;
        _sysOrgService = sysOrgService;
        _sysUserOrgService = sysUserOrgService;
        _sysUserRoleService = sysUserRoleService;
        _sysCacheService = sysCacheService;
        _sysUserExtOrgPosService = sysUserExtOrgPosService;
    }

    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysUser/page")]
    public async Task<SqlSugarPagedList<SysUser>> GetUserPage([FromQuery] PageUserInput input)
    {
        var orgList = input.OrgId > 0 ? await _sysOrgService.GetChildIdListWithSelfById(input.OrgId) :
            _userManager.SuperAdmin ? null : await _sysOrgService.GetChildIdListWithSelfById(_userManager.User.OrgId);

        return await _sysUserRep.AsQueryable()
            .WhereIF(!_userManager.SuperAdmin, u => u.UserType != UserTypeEnum.SuperAdmin)
            .WhereIF(orgList != null, u => orgList.Contains(u.OrgId))
            .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone))
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/add")]
    public async Task AddUser(AddUserInput input)
    {
        CheckDataScope(input.OrgId); // 数据范围检查

        var isExist = await _sysUserRep.IsAnyAsync(u => u.UserName == input.UserName);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1003);

        var user = input.Adapt<SysUser>();
        user.Password = MD5Encryption.Encrypt(CommonConst.SysPassword);
        await _sysUserRep.InsertAsync(user);
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/update")]
    public async Task UpdateUser(UpdateUserInput input)
    {
        CheckDataScope(input.OrgId); // 数据范围检查

        var isExist = await _sysUserRep.IsAnyAsync(u => u.UserName == input.UserName && u.Id != input.Id);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1003);

        await _sysUserRep.AsUpdateable(input.Adapt<SysUser>()).IgnoreColumns(true)
            .IgnoreColumns(u => new { u.UserType, u.Password, u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/delete")]
    public async Task DeleteUser(DeleteUserInput input)
    {
        CheckDataScope(input.OrgId); // 数据范围检查

        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1002);
        if (user.UserType == UserTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1014);
        //if (user.UserType == UserTypeEnum.Admin)
        //    throw Oops.Oh(ErrorCodeEnum.D1018);
        if (user.Id == _userManager.UserId)
            throw Oops.Oh(ErrorCodeEnum.D1001);

        await _sysUserRep.DeleteAsync(user);

        //// 删除用户-附属机构职位信息
        await _sysUserExtOrgPosService.DeleteEmpExtByUserId(input.Id);

        //删除用户-角色关联信息
        await _sysUserRoleService.DeleteUserRoleByUserId(input.Id);

        //删除用户-机构关联信息
        await _sysUserOrgService.DeleteUserOrgByUserId(input.Id);
    }

    /// <summary>
    /// 查看用户
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysUser/detail")]
    public async Task<SysUser> GetUser(long id)
    {
        return await _sysUserRep.GetFirstAsync(u => u.Id == id);
    }

    /// <summary>
    /// 设置用户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/setStatus")]
    public async Task<int> SetUserStatus(UserInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        if (user.UserType == UserTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1015);

        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        user.Status = (StatusEnum)input.Status;
        return await _sysUserRep.AsUpdateable(user)
            .UpdateColumns(u => new { u.Status }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/grantRole")]
    public async Task GrantUserRole(UserRoleInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        if (user.UserType == UserTypeEnum.SuperAdmin)
            throw Oops.Oh(ErrorCodeEnum.D1022);

        //if (user.UserType == UserTypeEnum.Admin)
        //    throw Oops.Oh(ErrorCodeEnum.D1008);

        CheckDataScope(input.OrgId); // 数据范围检查
        await _sysUserRoleService.GrantUserRole(input);
    }

    /// <summary>
    /// 授权用户机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/grantOrg")]
    public async Task GrantUserOrg(UserOrgInput input)
    {
        _sysCacheService.Remove(CacheConst.KeyOrgIdList + $"{input.Id}"); // 清除缓存

        CheckDataScope(input.OrgId); // 数据范围检查
        await _sysUserOrgService.GrantUserOrg(input);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/changeUserPwd")]
    public async Task<int> ChangeUserPwd(ChangePwdInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        if (MD5Encryption.Encrypt(input.PasswordOld) != user.Password)
            throw Oops.Oh(ErrorCodeEnum.D1004);
        user.Password = MD5Encryption.Encrypt(input.PasswordNew);
        return await _sysUserRep.AsUpdateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysUser/resetPwd")]
    public async Task<int> ResetUserPwd(ResetPwdUserInput input)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == input.Id);
        user.Password = MD5Encryption.Encrypt(CommonConst.SysPassword);
        return await _sysUserRep.AsUpdateable(user).UpdateColumns(u => u.Password).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取用户拥有角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysUser/ownRole")]
    public async Task<List<long>> GetUserOwnRole([FromQuery] UserInput input)
    {
        return await _sysUserRoleService.GetUserRoleIdList(input.Id);
    }

    /// <summary>
    /// 获取用户拥有机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysUser/ownOrg")]
    public async Task<List<long>> GetUserOwnOrg([FromQuery] UserInput input)
    {
        return await _sysUserOrgService.GetUserOrgIdList(input.Id);
    }

    /// <summary>
    /// 获取当前用户机构列表权限
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetUserOrgIdList()
    {
        return await _sysOrgService.GetUserOrgIdList();
    }

    /// <summary>
    /// 检查用户数据范围
    /// 当有多个机构时，在登录时选择一个组织，所以组织Id/OrgId从前端传过来
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    private async void CheckDataScope(long orgId)
    {
        if (!_userManager.SuperAdmin)
        {
            var dataScopes = await GetUserOrgIdList();
            if (!dataScopes.Any(u => u == orgId))
                throw Oops.Oh(ErrorCodeEnum.D1013);
        }
    }
}