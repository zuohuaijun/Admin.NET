using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core.SeedData
{
    public class SysUserDataScopeSeedData : IEntitySeedData<SysUserDataScope>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysUserDataScope> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysUserDataScope{SysUserId=142307070910554, SysOrgId=142307070910547 }
            };
        }
    }
}