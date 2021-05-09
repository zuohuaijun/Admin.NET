using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
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
                new SysEmpPos{SysEmpId=142307070910551, SysPosId=142307070910547 },
                new SysEmpPos{SysEmpId=142307070910551, SysPosId=142307070910548 },
                new SysEmpPos{SysEmpId=142307070910552, SysPosId=142307070910549 },
                new SysEmpPos{SysEmpId=142307070910553, SysPosId=142307070910547 },

                new SysEmpPos{SysEmpId=142307070910554, SysPosId=142307070910551 },
                new SysEmpPos{SysEmpId=142307070910554, SysPosId=142307070910552 },
                new SysEmpPos{SysEmpId=142307070910555, SysPosId=142307070910553 },
                new SysEmpPos{SysEmpId=142307070910555, SysPosId=142307070910551 },

                new SysEmpPos{SysEmpId=142307070910557, SysPosId=142307070910555 },
                new SysEmpPos{SysEmpId=142307070910557, SysPosId=142307070910556 },
                new SysEmpPos{SysEmpId=142307070910558, SysPosId=142307070910557 },
                new SysEmpPos{SysEmpId=142307070910559, SysPosId=142307070910555 }
            };
        }
    }
}