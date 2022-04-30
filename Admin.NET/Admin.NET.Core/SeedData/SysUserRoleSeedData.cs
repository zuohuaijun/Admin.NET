using System.Collections.Generic;

namespace Admin.NET.Core
{
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
                new SysUserRole{ Id=252885263003000, UserId=252885263003721, RoleId=252885263003721 },
                new SysUserRole{ Id=252885263003001, UserId=252885263003722, RoleId=252885263003722 },
                new SysUserRole{ Id=252885263003002, UserId=252885263003723, RoleId=252885263003724 }
            };
        }
    }
}