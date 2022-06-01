namespace Admin.NET.Core;

/// <summary>
/// 系统用户附属机构职位表种子数据
/// </summary>
public class SysEmpExtOrgPosSeedData : ISqlSugarEntitySeedData<SysUserExtOrgPos>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysUserExtOrgPos> HasData()
    {
        return new[]
        {
            new SysUserExtOrgPos{ Id=252885263003000, UserId=252885263003721, OrgId=252885263003730, PosId=252885263003720 },
            new SysUserExtOrgPos{ Id=252885263003001, UserId=252885263003722, OrgId=252885263003740, PosId=252885263003722  }
        };
    }
}