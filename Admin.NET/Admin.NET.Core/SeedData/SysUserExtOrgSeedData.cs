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
    [IgnoreUpdate]
    public IEnumerable<SysUserExtOrg> HasData()
    {
        return new[]
        {
            new SysUserExtOrg{ Id=1300000000101, UserId=1300000000111, OrgId=1300000000202, PosId=1300000000106 },
            new SysUserExtOrg{ Id=1300000000102, UserId=1300000000114, OrgId=1300000000302, PosId=1300000000108  }
        };
    }
}