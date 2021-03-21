using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统员工职位表种子数据
    /// </summary>
    public class SysEmpPosSeedData : IEntitySeedData<SysEmpPos>
    {
        /// <summary>
        /// 员工种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysEmpPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysEmpPos{SysEmpId=1, SysPosId=1 },
                new SysEmpPos{SysEmpId=1, SysPosId=2 },
                new SysEmpPos{SysEmpId=2, SysPosId=3 },
                new SysEmpPos{SysEmpId=3, SysPosId=1 }
            };
        }
    }
}
