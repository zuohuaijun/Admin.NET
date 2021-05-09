using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统职位表种子数据
    /// </summary>
    public class SysPosSeedData : IEntitySeedData<SysPos>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysPos{TenantId=142307070918780, Id=142307070910547, Name="总经理", Code="zjl", Sort=100, Remark="总经理", Status=0 },
                new SysPos{TenantId=142307070918780, Id=142307070910548, Name="副总经理", Code="fzjl", Sort=101, Remark="副总经理", Status=0 },
                new SysPos{TenantId=142307070918780, Id=142307070910549, Name="部门经理", Code="bmjl", Sort=102, Remark="部门经理", Status=0 },
                new SysPos{TenantId=142307070918780, Id=142307070910550, Name="工作人员", Code="gzry", Sort=103, Remark="工作人员", Status=0 },

                new SysPos{TenantId=142307070918781, Id=142307070910551, Name="总经理", Code="zjl", Sort=100, Remark="总经理", Status=0 },
                new SysPos{TenantId=142307070918781, Id=142307070910552, Name="副总经理", Code="fzjl", Sort=101, Remark="副总经理", Status=0 },
                new SysPos{TenantId=142307070918781, Id=142307070910553, Name="部门经理", Code="bmjl", Sort=102, Remark="部门经理", Status=0 },
                new SysPos{TenantId=142307070918781, Id=142307070910554, Name="工作人员", Code="gzry", Sort=103, Remark="工作人员", Status=0 },

                new SysPos{TenantId=142307070918782, Id=142307070910555, Name="总经理", Code="zjl", Sort=100, Remark="总经理", Status=0 },
                new SysPos{TenantId=142307070918782, Id=142307070910556, Name="副总经理", Code="fzjl", Sort=101, Remark="副总经理", Status=0 },
                new SysPos{TenantId=142307070918782, Id=142307070910557, Name="部门经理", Code="bmjl", Sort=102, Remark="部门经理", Status=0 },
                new SysPos{TenantId=142307070918782, Id=142307070910558, Name="工作人员", Code="gzry", Sort=103, Remark="工作人员", Status=0 }
            };
        }
    }
}