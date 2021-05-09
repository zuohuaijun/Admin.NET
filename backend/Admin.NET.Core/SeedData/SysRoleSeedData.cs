using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
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
                new SysRole{TenantId=142307070918780, Id=142307070910554, Name="系统管理员", Code="sys_manager_role", Sort=100, DataScopeType=DataScopeType.ALL, Remark="系统管理员", Status=0 },
                new SysRole{TenantId=142307070918780, Id=142307070910555, Name="普通用户", Code="common_role", Sort=101, DataScopeType=DataScopeType.DEFINE, Remark="普通用户", Status=0 },

                new SysRole{TenantId=142307070918781, Id=142307070910556, Name="系统管理员", Code="sys_manager_role", Sort=100, DataScopeType=DataScopeType.DEFINE, Remark="系统管理员", Status=0 },
                new SysRole{TenantId=142307070918781, Id=142307070910557, Name="普通用户", Code="common_role", Sort=101, DataScopeType=DataScopeType.DEFINE, Remark="普通用户", Status=0 },
            };
        }
    }
}