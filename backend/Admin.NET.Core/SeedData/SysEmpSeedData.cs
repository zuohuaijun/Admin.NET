using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
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
                new SysEmp{Id=142307070910551, JobNum="D1001", OrgId=142307070910539, OrgName="华夏集团" },
                new SysEmp{Id=142307070910552, JobNum="D1002", OrgId=142307070910539, OrgName="华夏集团" },
                new SysEmp{Id=142307070910553, JobNum="D1003", OrgId=142307070910539, OrgName="华夏集团" },

                new SysEmp{Id=142307070910554, JobNum="D1001", OrgId=142307070910547, OrgName="租户1公司" },
                new SysEmp{Id=142307070910555, JobNum="D1002", OrgId=142307070910547, OrgName="租户1公司" },
                new SysEmp{Id=142307070910556, JobNum="D1003", OrgId=142307070910547, OrgName="租户1公司" },

                new SysEmp{Id=142307070910557, JobNum="D1001", OrgId=142307070910548, OrgName="租户2公司" },
                new SysEmp{Id=142307070910558, JobNum="D1002", OrgId=142307070910548, OrgName="租户2公司" },
                new SysEmp{Id=142307070910559, JobNum="D1003", OrgId=142307070910548, OrgName="租户2公司" }
            };
        }
    }
}