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
                new SysEmp{Id=1, JobNum="D1001", OrgId=1, OrgName="河北省" },
                new SysEmp{Id=2, JobNum="D1002", OrgId=1, OrgName="河北省" },
                new SysEmp{Id=3, JobNum="D1003", OrgId=1, OrgName="河北省" }
            };
        }
    }
}
