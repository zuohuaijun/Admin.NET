namespace Admin.NET.Core.Service;

/// <summary>
/// 系统用户机构服务
/// </summary>
public class SysUserOrgService : ITransient
{
    private readonly SqlSugarRepository<SysUserOrg> _sysUserOrgRep;

    public SysUserOrgService(SqlSugarRepository<SysUserOrg> sysUserOrgRep)
    {
        _sysUserOrgRep = sysUserOrgRep;
    }

    /// <summary>
    /// 授权用户所属机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [SqlSugarUnitOfWork]
    public async Task GrantUserOrg(UserOrgInput input)
    {
        await _sysUserOrgRep.DeleteAsync(u => u.UserId == input.Id);
        var userOrgs = input.OrgIdList.Select(u => new SysUserOrg
        {
            UserId = input.Id,
            OrgId = u
        }).ToList();
        await _sysUserOrgRep.InsertRangeAsync(userOrgs);
    }

    /// <summary>
    /// 根据用户Id获取机构Id集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetUserOrgIdList(long userId)
    {
        return await _sysUserOrgRep.AsQueryable()
            .Where(u => u.UserId == userId)
            .Select(u => u.OrgId).ToListAsync();
    }

    /// <summary>
    /// 根据机构Id集合删除用户机构
    /// </summary>
    /// <param name="orgIdList"></param>
    /// <returns></returns>
    public async Task DeleteUserOrgByOrgIdList(List<long> orgIdList)
    {
        await _sysUserOrgRep.DeleteAsync(u => orgIdList.Contains(u.OrgId));
    }

    /// <summary>
    /// 根据用户Id删除用户机构
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteUserOrgByUserId(long userId)
    {
        await _sysUserOrgRep.DeleteAsync(u => u.UserId == userId);
    }

    /// <summary>
    /// 增加用户机构
    /// </summary>
    /// <returns></returns>
    public async Task AddUserOrg(SysUserOrg userOrg)
    {
        await _sysUserOrgRep.InsertAsync(userOrg);
    }
}