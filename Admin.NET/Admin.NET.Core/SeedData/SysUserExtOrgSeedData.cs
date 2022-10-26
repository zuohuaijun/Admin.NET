namespace Admin.NET.Core;

/// <summary>
/// 系统用户扩展机构表种子数据
/// </summary>
public class SysUserExtOrgSeedData : ISqlSugarEntitySeedData<SysUserExtOrg>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysUserExtOrg> HasData()
    {
        return new[]
        {
            new SysUserExtOrg{ Id=252885263003000, UserId=252885263003721, OrgId=252885263003730, PosId=252885263003720 },
            new SysUserExtOrg{ Id=252885263003001, UserId=252885263003722, OrgId=252885263003740, PosId=252885263003722  }
        };
    }
}