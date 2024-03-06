// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 系统用户角色表种子数据
/// </summary>
public class SysUserRoleSeedData : ISqlSugarEntitySeedData<SysUserRole>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysUserRole> HasData()
    {
        return new[]
        {
            new SysUserRole{ Id=1300000000101, UserId=1300000000111, RoleId=1300000000101 },
            new SysUserRole{ Id=1300000000102, UserId=1300000000112, RoleId=1300000000102 },
            new SysUserRole{ Id=1300000000103, UserId=1300000000113, RoleId=1300000000103 },
            new SysUserRole{ Id=1300000000104, UserId=1300000000114, RoleId=1300000000104 },
            new SysUserRole{ Id=1300000000105, UserId=1300000000115, RoleId=1300000000105 },
        };
    }
}