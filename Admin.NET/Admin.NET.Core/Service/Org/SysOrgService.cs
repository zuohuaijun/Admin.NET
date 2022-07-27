namespace Admin.NET.Core.Service;

/// <summary>
/// 系统机构服务
/// </summary>
[ApiDescriptionSettings(Name = "系统机构", Order = 197)]
public class SysOrgService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysOrg> _sysOrgRep;
    private readonly IUserManager _userManager;
    private readonly ISysCacheService _sysCacheService;
    private readonly SysUserOrgService _sysUserOrgService;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly SysRoleOrgService _sysRoleOrgService;
    private readonly SysUserExtOrgPosService _sysEmpExtOrgPosService;

    public SysOrgService(SqlSugarRepository<SysOrg> sysOrgRep,
        IUserManager userManager,
        ISysCacheService sysCacheService,
        SysUserOrgService sysUserOrgService,
        SysUserRoleService sysUserRoleService,
        SysRoleOrgService sysRoleOrgService,
        SysUserExtOrgPosService sysEmpExtOrgPosService)
    {
        _sysOrgRep = sysOrgRep;
        _userManager = userManager;
        _sysCacheService = sysCacheService;
        _sysUserOrgService = sysUserOrgService;
        _sysUserRoleService = sysUserRoleService;
        _sysRoleOrgService = sysRoleOrgService;
        _sysEmpExtOrgPosService = sysEmpExtOrgPosService;
    }

    /// <summary>
    /// 获取用户拥有机构信息列表
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    [HttpGet("/sysOrg/userOwnOrgInfo/{userId}")]
    public async Task<List<SysOrg>> GetUserOrgList(long userId)
    {
        List<long> orgList = await _sysUserOrgService.GetUserOrgIdList(userId);
        return await _sysOrgRep.AsQueryable().Where(t => orgList.Contains(t.Id)).ToListAsync();
    }

    /// <summary>
    /// 获取机构列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysOrg/list")]
    public async Task<List<SysOrg>> GetOrgList([FromQuery] OrgInput input)
    {
        var orgIdList = input.Id > 0 ? await GetChildIdListWithSelfById(input.Id) : await GetUserOrgIdList();

        var iSugarQueryable = _sysOrgRep.AsQueryable().OrderBy(u => u.Order)
            .WhereIF(orgIdList.Count > 0, u => orgIdList.Contains(u.Id)); // 非超级管理员限制

        if (!string.IsNullOrWhiteSpace(input.Name) || !string.IsNullOrWhiteSpace(input.Code) || input.Id > 0)
        {
            iSugarQueryable = iSugarQueryable
                .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code));
        }
        return await iSugarQueryable.ToTreeAsync(u => u.Children, u => u.Pid, input.Id > 0 ? input.Id : 0);
    }

    /// <summary>
    /// 增加机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysOrg/add")]
    public async Task<long> AddOrg(AddOrgInput input)
    {
        var isExist = await _sysOrgRep.IsAnyAsync(u => u.Code == input.Code && u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D2002);

        var orgIdList = await GetUserOrgIdList();
        if (!_userManager.SuperAdmin)
        {
            // 新增机构父Id不是0，则进行权限校验
            if (input.Pid != 0)
            {
                // 新增机构的父机构不在自己的数据范围内
                if (orgIdList.Count < 1 || !orgIdList.Contains(input.Pid))
                    throw Oops.Oh(ErrorCodeEnum.D2003);
            }
            else
                throw Oops.Oh(ErrorCodeEnum.D2006);
        }
        var sysOrg = input.Adapt<SysOrg>();
        var newOrg = await _sysOrgRep.AsInsertable(sysOrg).ExecuteReturnEntityAsync();

        // 非超级管理员时，将新机构加到用户数据范围内
        if (!_userManager.SuperAdmin)
        {
            var userId = _userManager.UserId;
            await _sysUserOrgService.AddUserOrg(new SysUserOrg
            {
                UserId = userId,
                OrgId = newOrg.Id
            });
            orgIdList.Add(newOrg.Id);
            await _sysCacheService.SetOrgIdList(userId, orgIdList); // 刷新缓存
        }
        return newOrg.Id;
    }

    /// <summary>
    /// 更新机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysOrg/update")]
    [UnitOfWork]
    public async Task UpdateOrg(UpdateOrgInput input)
    {
        if (input.Pid != 0)
        {
            var pOrg = await _sysOrgRep.GetFirstAsync(u => u.Id == input.Pid);
            _ = pOrg ?? throw Oops.Oh(ErrorCodeEnum.D2000);
        }
        if (input.Id == input.Pid)
            throw Oops.Oh(ErrorCodeEnum.D2001);

        var sysOrg = await _sysOrgRep.GetFirstAsync(u => u.Id == input.Id);
        var isExist = await _sysOrgRep.IsAnyAsync(u => (u.Name == input.Name && u.Code == input.Code) && u.Id != sysOrg.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D2002);

        // 父Id不能为自己的子节点
        var childIdList = await GetChildIdListWithSelfById(input.Id);
        if (childIdList.Contains(input.Pid))
            throw Oops.Oh(ErrorCodeEnum.D2001);

        // 是否有权限操作此机构
        var dataScopes = await GetUserOrgIdList();
        if (!_userManager.SuperAdmin && (dataScopes.Count < 1 || !dataScopes.Contains(sysOrg.Id)))
            throw Oops.Oh(ErrorCodeEnum.D2003);

        await _sysOrgRep.AsUpdateable(input.Adapt<SysOrg>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysOrg/delete")]
    public async Task DeleteOrg(DeleteOrgInput input)
    {
        var sysOrg = await _sysOrgRep.GetFirstAsync(u => u.Id == input.Id);

        // 是否有权限操作此机构
        if (!_userManager.SuperAdmin)
        {
            var dataScopes = await GetUserOrgIdList();
            if (dataScopes.Count < 1 || !dataScopes.Contains(sysOrg.Id))
                throw Oops.Oh(ErrorCodeEnum.D2003);
        }

        // 若子机构有用户则禁止删除
        var orgTreeList = await _sysOrgRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id);
        var orgIdList = orgTreeList.Select(u => u.Id).ToList();

        // 若附属机构有用户则禁止删除
        var hasExtOrgEmp = await _sysEmpExtOrgPosService.HasExtOrgEmp(sysOrg.Id);
        if (hasExtOrgEmp)
            throw Oops.Oh(ErrorCodeEnum.D2005);

        // 级联删除机构子节点
        await _sysOrgRep.DeleteAsync(u => orgIdList.Contains(u.Id));

        // 级联删除角色机构数据
        await _sysRoleOrgService.DeleteRoleOrgByOrgIdList(orgIdList);

        // 级联删除用户机构数据
        await _sysUserOrgService.DeleteUserOrgByOrgIdList(orgIdList);
    }

    /// <summary>
    /// 根据用户Id获取机构Id集合
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetUserOrgIdList()
    {
        if (_userManager.SuperAdmin)
            return new List<long>();

        var userId = _userManager.UserId;
        var orgIdList = await _sysCacheService.GetOrgIdList(userId); // 取缓存
        if (orgIdList == null || orgIdList.Count < 1)
        {
            // 用户机构集合
            var orgList1 = await _sysUserOrgService.GetUserOrgIdList(userId);
            // 角色机构集合
            var orgList2 = await GetUserRoleOrgIdList(userId);
            // 并集机构集合
            orgIdList = orgList1.Concat(orgList2).Distinct().ToList();
            await _sysCacheService.SetOrgIdList(userId, orgIdList); // 存缓存
        }
        return orgIdList;
    }

    /// <summary>
    /// 获取用户角色机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<List<long>> GetUserRoleOrgIdList(long userId)
    {
        var roleList = await _sysUserRoleService.GetUserRoleList(userId);
        if (roleList.Count < 1)
            return new List<long>(); // 空机构Id集合

        return await GetUserOrgIdList(roleList);
    }

    /// <summary>
    /// 根据角色Id集合获取机构Id集合
    /// </summary>
    /// <param name="roleList"></param>
    /// <returns></returns>
    private async Task<List<long>> GetUserOrgIdList(List<SysRole> roleList)
    {
        // 按最大范围策略设定(如果同时拥有ALL和SELF的权限，则结果ALL)
        int strongerDataScopeType = (int)DataScopeEnum.Self;

        // 数据范围拥有的角色集合
        var customDataScopeRoleIdList = new List<long>();
        if (roleList != null && roleList.Count > 0)
        {
            roleList.ForEach(u =>
            {
                if (u.DataScope == DataScopeEnum.Define)
                    customDataScopeRoleIdList.Add(u.Id);
                else if ((int)u.DataScope <= strongerDataScopeType)
                    strongerDataScopeType = (int)u.DataScope;
            });
        }

        // 根据角色集合获取机构集合
        var orgIdList1 = await _sysRoleOrgService.GetRoleOrgIdList(customDataScopeRoleIdList);
        // 根据数据范围获取机构集合
        var orgIdList2 = await GetOrgIdListByDataScope(strongerDataScopeType);
        // 并集机构集合
        return orgIdList1.Concat(orgIdList2).Distinct().ToList();
    }

    /// <summary>
    /// 根据数据范围获取机构Id集合
    /// </summary>
    /// <param name="dataScope"></param>
    /// <returns></returns>
    private async Task<List<long>> GetOrgIdListByDataScope(int dataScope)
    {
        var orgId = _userManager.User.OrgId;
        var orgIdList = new List<long>();
        // 若数据范围是全部，则获取所有机构Id集合
        if (dataScope == (int)DataScopeEnum.All)
        {
            orgIdList = await _sysOrgRep.AsQueryable().Select(u => u.Id).ToListAsync();
        }
        // 若数据范围是本部门及以下，则获取本节点和子节点集合
        else if (dataScope == (int)DataScopeEnum.Dept_with_child)
        {
            orgIdList = await GetChildIdListWithSelfById(orgId);
        }
        // 若数据范围是本部门不含子节点，则直接返回本部门
        else if (dataScope == (int)DataScopeEnum.Dept)
        {
            orgIdList.Add(orgId);
        }
        return orgIdList;
    }

    /// <summary>
    /// 根据节点Id获取子节点Id集合(包含自己)
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<List<long>> GetChildIdListWithSelfById(long pid)
    {
        var orgTreeList = await _sysOrgRep.AsQueryable().ToChildListAsync(u => u.Pid, pid);
        return orgTreeList.Select(u => u.Id).ToList();
    }
}