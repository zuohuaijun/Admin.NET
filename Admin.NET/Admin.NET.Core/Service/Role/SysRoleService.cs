// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统角色服务
/// </summary>
[ApiDescriptionSettings(Order = 480)]
public class SysRoleService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysRole> _sysRoleRep;
    private readonly SysRoleOrgService _sysRoleOrgService;
    private readonly SysRoleMenuService _sysRoleMenuService;
    private readonly SysOrgService _sysOrgService;
    private readonly SysUserRoleService _sysUserRoleService;

    public SysRoleService(UserManager userManager,
        SqlSugarRepository<SysRole> sysRoleRep,
        SysRoleOrgService sysRoleOrgService,
        SysRoleMenuService sysRoleMenuService,
        SysOrgService sysOrgService,
        SysUserRoleService sysUserRoleService)
    {
        _userManager = userManager;
        _sysRoleRep = sysRoleRep;
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
    [DisplayName("获取角色分页列表")]
    public async Task<SqlSugarPagedList<SysRole>> Page(PageRoleInput input)
    {
        return await _sysRoleRep.AsQueryable()
            .WhereIF(!_userManager.SuperAdmin, u => u.CreateUserId == _userManager.UserId) // 若非超管，则只能操作自己创建的角色
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取角色列表")]
    public async Task<List<RoleOutput>> GetList()
    {
        // 当前用户已拥有的角色集合
        var roleIdList = _userManager.SuperAdmin ? null : await _sysUserRoleService.GetUserRoleIdList(_userManager.UserId);

        return await _sysRoleRep.AsQueryable()
            .WhereIF(roleIdList != null, u => u.CreateUserId == _userManager.UserId || roleIdList.Contains(u.Id)) // 若非超管，则只显示自己创建和已拥有的角色
            .OrderBy(u => u.OrderNo).Select<RoleOutput>().ToListAsync();
    }

    /// <summary>
    /// 增加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加角色")]
    public async Task AddRole(AddRoleInput input)
    {
        if (await _sysRoleRep.IsAnyAsync(u => u.Name == input.Name && u.Code == input.Code))
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var newRole = await _sysRoleRep.AsInsertable(input.Adapt<SysRole>()).ExecuteReturnEntityAsync();
        input.Id = newRole.Id;
        await UpdateRoleMenu(input);
    }

    /// <summary>
    /// 更新角色菜单权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task UpdateRoleMenu(AddRoleInput input)
    {
        if (input.MenuIdList == null || input.MenuIdList.Count < 1)
            return;

        // 将父节点为0的菜单排除，防止前端全选异常
        var pMenuIds = await _sysRoleRep.ChangeRepository<SqlSugarRepository<SysMenu>>().AsQueryable().Where(u => input.MenuIdList.Contains(u.Id) && u.Pid == 0).ToListAsync(u => u.Id);
        var menuIds = input.MenuIdList.Except(pMenuIds); // 差集
        await GrantMenu(new RoleMenuInput()
        {
            Id = input.Id,
            MenuIdList = menuIds.ToList()
        });
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新角色")]
    public async Task UpdateRole(UpdateRoleInput input)
    {
        if (await _sysRoleRep.IsAnyAsync(u => u.Name == input.Name && u.Code == input.Code && u.Id != input.Id))
            throw Oops.Oh(ErrorCodeEnum.D1006);

        await _sysRoleRep.AsUpdateable(input.Adapt<SysRole>()).IgnoreColumns(true)
            .IgnoreColumns(u => new { u.DataScope }).ExecuteCommandAsync();

        await UpdateRoleMenu(input);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除角色")]
    public async Task DeleteRole(DeleteRoleInput input)
    {
        var sysRole = await _sysRoleRep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        if (sysRole.Code == CommonConst.SysAdminRole)
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
    [DisplayName("授权角色菜单")]
    public async Task GrantMenu(RoleMenuInput input)
    {
        await _sysRoleMenuService.GrantRoleMenu(input);
    }

    /// <summary>
    /// 授权角色数据范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("授权角色数据范围")]
    public async Task GrantDataScope(RoleOrgInput input)
    {
        // 删除与该角色相关的用户机构缓存
        var userIdList = await _sysUserRoleService.GetUserIdList(input.Id);
        foreach (var userId in userIdList)
        {
            SqlSugarFilter.DeleteUserOrgCache(userId, _sysRoleRep.Context.CurrentConnectionConfig.ConfigId.ToString());
        }

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
    /// 根据角色Id获取菜单Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("根据角色Id获取菜单Id集合")]
    public async Task<List<long>> GetOwnMenuList([FromQuery] RoleInput input)
    {
        return await _sysRoleMenuService.GetRoleMenuIdList(new List<long> { input.Id });
    }

    /// <summary>
    /// 根据角色Id获取机构Id集合
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("根据角色Id获取机构Id集合")]
    public async Task<List<long>> GetOwnOrgList([FromQuery] RoleInput input)
    {
        return await _sysRoleOrgService.GetRoleOrgIdList(new List<long> { input.Id });
    }

    /// <summary>
    /// 设置角色状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("设置角色状态")]
    public async Task<int> SetStatus(RoleInput input)
    {
        if (!Enum.IsDefined(typeof(StatusEnum), input.Status))
            throw Oops.Oh(ErrorCodeEnum.D3005);

        return await _sysRoleRep.AsUpdateable()
            .SetColumns(u => u.Status == input.Status)
            .Where(u => u.Id == input.Id)
            .ExecuteCommandAsync();
    }
}