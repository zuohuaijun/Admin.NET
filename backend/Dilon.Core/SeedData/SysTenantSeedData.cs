using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    public class SysTenantSeedData : IEntitySeedData<SysTenant, MultiTenantDbContextLocator>
    {
        public IEnumerable<SysTenant> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new List<SysTenant>
            {
                new SysTenant
                {
                    Id = 142307070918780,
                    Name = "默认租户",
                    Host = "localhost:5566",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                    Connection = "Data Source=./Dilon.db",
                    Email = "zuohuaijun@163.com",
                    Phone = "18020030720"
                },
                new SysTenant
                {
                    Id = 142307070918781,
                    Name = "其他租户",
                    Host = "localhost:5588",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                    Connection = "Data Source=./Dilon_1.db"
                }
            };
        }
    }
}
