using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core.SeedData
{
    public class SysUserRoleSeedData : IEntitySeedData<SysUserRole>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysUserRole> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                // 租户管理员默认管理员角色
                new SysUserRole{SysUserId=142307070910554, SysRoleId=142307070910556 }
            };
        }
    }
}