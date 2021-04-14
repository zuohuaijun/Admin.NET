using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统员工表种子数据
    /// </summary>
    public class SysEmpSeedData : IEntitySeedData<SysEmp>
    {
        /// <summary>
        /// 员工种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysEmp> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysEmp{TenantId=142307070918780,Id=142307070910551, JobNum="D1001", OrgId=142307070910539, OrgName="华夏集团" },
                new SysEmp{TenantId=142307070918780,Id=142307070910552, JobNum="D1002", OrgId=142307070910539, OrgName="华夏集团" },
                new SysEmp{TenantId=142307070918780,Id=142307070910553, JobNum="D1003", OrgId=142307070910539, OrgName="华夏集团" }
            };
        }
    }
}
