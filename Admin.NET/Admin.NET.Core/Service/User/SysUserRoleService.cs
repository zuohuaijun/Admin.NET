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
/// 系统用户角色服务
/// </summary>
public class SysUserRoleService : ITransient
{
    private readonly SqlSugarRepository<SysUserRole> _sysUserRoleRep;
    private readonly SysCacheService _sysCacheService;

    public SysUserRoleService(SqlSugarRepository<SysUserRole> sysUserRoleRep,
        SysCacheService sysCacheService)
    {
        _sysUserRoleRep = sysUserRoleRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork]
    public async Task GrantUserRole(UserRoleInput input)
    {
        await _sysUserRoleRep.DeleteAsync(u => u.UserId == input.UserId);

        if (input.RoleIdList == null || input.RoleIdList.Count < 1) return;
        var roles = input.RoleIdList.Select(u => new SysUserRole
        {
            UserId = input.UserId,
            RoleId = u
        }).ToList();
        await _sysUserRoleRep.InsertRangeAsync(roles);
        _sysCacheService.Remove(CacheConst.KeyUserButton + input.UserId);
    }

    /// <summary>
    /// 根据角色Id删除用户角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task DeleteUserRoleByRoleId(long roleId)
    {
        await _sysUserRoleRep.AsQueryable()
             .Where(u => u.RoleId == roleId)
             .Select(u => u.UserId)
             .ForEachAsync(userId =>
             {
                 _sysCacheService.Remove(CacheConst.KeyUserButton + userId);
             });

        await _sysUserRoleRep.DeleteAsync(u => u.RoleId == roleId);
    }

    /// <summary>
    /// 根据用户Id删除用户角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteUserRoleByUserId(long userId)
    {
        await _sysUserRoleRep.DeleteAsync(u => u.UserId == userId);
        _sysCacheService.Remove(CacheConst.KeyUserButton + userId);
    }

    /// <summary>
    /// 根据用户Id获取角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<SysRole>> GetUserRoleList(long userId)
    {
        var sysUserRoleList = await _sysUserRoleRep.AsQueryable()
            .Includes(u => u.SysRole)
            .Where(u => u.UserId == userId).ToListAsync();
        return sysUserRoleList.Where(u => u.SysRole != null).Select(u => u.SysRole).ToList();
    }

    /// <summary>
    /// 根据用户Id获取角色Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetUserRoleIdList(long userId)
    {
        return await _sysUserRoleRep.AsQueryable()
            .Where(u => u.UserId == userId).Select(u => u.RoleId).ToListAsync();
    }

    /// <summary>
    /// 根据角色Id获取用户Id集合
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetUserIdList(long roleId)
    {
        return await _sysUserRoleRep.AsQueryable()
            .Where(u => u.RoleId == roleId).Select(u => u.UserId).ToListAsync();
    }
}