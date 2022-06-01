namespace Admin.NET.Core.Service;

/// <summary>
/// 系统用户附属机构职位服务
/// </summary>
public class SysUserExtOrgPosService : ITransient
{
    private readonly SqlSugarRepository<SysUserExtOrgPos> _sysEmpExtOrgPosRep;

    public SysUserExtOrgPosService(SqlSugarRepository<SysUserExtOrgPos> sysEmpExtOrgPosRep)
    {
        _sysEmpExtOrgPosRep = sysEmpExtOrgPosRep;
    }

    /// <summary>
    /// 保存或编辑附属机构信息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="extIdList"></param>
    /// <returns></returns>
    [SqlSugarUnitOfWork]
    public async Task AddOrUpdate(long userId, List<UserExtOrgPosOutput> extIdList)
    {
        await DeleteEmpExtByUserId(userId); // 先删除
        var extOrgPos = extIdList.Select(u => new SysUserExtOrgPos
        {
            UserId = userId,
            OrgId = u.OrgId,
            PosId = u.PosId
        }).ToList();
        await _sysEmpExtOrgPosRep.InsertRangeAsync(extOrgPos);
    }

    /// <summary>
    /// 根据用户Id获取附属机构和职位信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<UserExtOrgPosOutput>> GetEmpExtOrgPosList(long userId)
    {
        return await _sysEmpExtOrgPosRep.AsQueryable()
            .Mapper(u => u.SysOrg, u => u.OrgId)
            .Mapper(u => u.SysPos, u => u.PosId)
            .Where(u => u.UserId == userId)
            .Select(u => new UserExtOrgPosOutput
            {
                OrgId = u.SysOrg.Id,
                OrgCode = u.SysOrg.Code,
                OrgName = u.SysOrg.Name,
                PosId = u.SysPos.Id,
                PosCode = u.SysPos.Code,
                PosName = u.SysPos.Name
            }).ToListAsync();
    }

    /// <summary>
    /// 根据附属机构Id判断是否有用户
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    public async Task<bool> HasExtOrgEmp(long orgId)
    {
        return await _sysEmpExtOrgPosRep.IsAnyAsync(u => u.OrgId == orgId);
    }

    /// <summary>
    /// 根据附属职位Id判断是否有用户
    /// </summary>
    /// <param name="posId"></param>
    /// <returns></returns>
    public async Task<bool> HasExtPosEmp(long posId)
    {
        return await _sysEmpExtOrgPosRep.IsAnyAsync(u => u.PosId == posId);
    }

    /// <summary>
    /// 根据用户Id删除用户附属
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteEmpExtByUserId(long userId)
    {
        await _sysEmpExtOrgPosRep.DeleteAsync(u => u.UserId == userId);
    }
}