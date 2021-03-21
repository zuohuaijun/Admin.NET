using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    class SysEmpExtOrgPosSeedData : IEntitySeedData<SysEmpExtOrgPos>
    {
        /// <summary>
        /// 扩展机构种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysEmpExtOrgPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysEmpExtOrgPos { SysEmpId = (long)1, SysOrgId = (long)1, SysPosId = (long)1 },
                new SysEmpExtOrgPos { SysEmpId = (long)1, SysOrgId = (long)2, SysPosId = (long)2 },
                new SysEmpExtOrgPos { SysEmpId = (long)1, SysOrgId = (long)3, SysPosId = (long)3 },
                new SysEmpExtOrgPos { SysEmpId = (long)1, SysOrgId = (long)4, SysPosId = (long)4 },
                new SysEmpExtOrgPos { SysEmpId = (long)2, SysOrgId = (long)1, SysPosId = (long)1 }
            };
        }
    }
}
