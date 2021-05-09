using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统员工附属机构职位表种子数据
    /// </summary>
    public class SysEmpExtOrgPosData : IEntitySeedData<SysEmpExtOrgPos>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysEmpExtOrgPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910539, SysPosId = 142307070910547 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910540, SysPosId = 142307070910548 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910541, SysPosId = 142307070910549 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910542, SysPosId = 142307070910550 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910553, SysOrgId = 142307070910542, SysPosId = 142307070910547 }
            };
        }
    }
}