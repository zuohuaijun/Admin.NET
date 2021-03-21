using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统角色表种子数据
    /// </summary>
    public class SysRoleSeedData : IEntitySeedData<SysRole>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysRole> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysRole{Id=1, Name="系统管理员", Code="sys_manager_role", Sort=100, DataScopeType=1, Remark="系统管理员", Status=0 },
                new SysRole{Id=2, Name="普通用户", Code="common_role", Sort=101, DataScopeType=5, Remark="普通用户", Status=0 }
            };
        }
    }
}
