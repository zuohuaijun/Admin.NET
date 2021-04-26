using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 租户种子数据
    /// </summary>
    public class SysTenantSeedData : IEntitySeedData<SysTenant, MultiTenantDbContextLocator>
    {
        public IEnumerable<SysTenant> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new List<SysTenant>
            {
                new SysTenant
                {
                    Id = 142307070918780,
                    Name = "平台开发",
                    Email = "515096995@163.com",
                    AdminName = "SuperAdmin",
                    Phone = "18020030720",
                    Host = "",
                    Connection = "",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                },
                new SysTenant
                {
                    Id = 142307070918781,
                    Name = "公司1租户",
                    Email = "zuohuaijun@163.com",
                    AdminName = "Admin",
                    Phone = "18020030720",
                    Host = "",
                    Connection = "",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                }
            };
        }
    }
}