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
                new SysEmpPos{Id=1,TenantId=142307070918780,SysEmpId=142307070910551, SysPosId=142307070910547 },
                new SysEmpPos{Id=2,TenantId=142307070918780,SysEmpId=142307070910551, SysPosId=142307070910548 },
                new SysEmpPos{Id=3,TenantId=142307070918780,SysEmpId=142307070910552, SysPosId=142307070910549 },
                new SysEmpPos{Id=4,TenantId=142307070918780,SysEmpId=142307070910553, SysPosId=142307070910547 },

                new SysEmpPos{Id=5,TenantId=142307070918781,SysEmpId=142307070910554, SysPosId=142307070910551 },
                new SysEmpPos{Id=6,TenantId=142307070918781,SysEmpId=142307070910554, SysPosId=142307070910552 },
                new SysEmpPos{Id=7,TenantId=142307070918781,SysEmpId=142307070910555, SysPosId=142307070910553 },
                new SysEmpPos{Id=8,TenantId=142307070918781,SysEmpId=142307070910555, SysPosId=142307070910551 },

                  new SysEmpPos{Id=9,TenantId=142307070918782,SysEmpId=142307070910557, SysPosId=142307070910555 },
                new SysEmpPos{Id=10,TenantId=142307070918782,SysEmpId=142307070910557, SysPosId=142307070910556 },
                new SysEmpPos{Id=11,TenantId=142307070918782,SysEmpId=142307070910558, SysPosId=142307070910557 },
                new SysEmpPos{Id=12,TenantId=142307070918782,SysEmpId=142307070910559, SysPosId=142307070910555 }

            };
        }
    }
}
