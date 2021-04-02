using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    public class TenantSeedData : IEntitySeedData<Tenant, MultiTenantDbContextLocator>
    {
        public IEnumerable<Tenant> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new List<Tenant>
            {
                new Tenant
                {
                    TenantId = Guid.Parse("2be54c55-a315-12d6-687c-ff711fcb0703"),
                    Name = "默认租户",
                    Host = "localhost:5566",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                    ConnectionString = "Data Source=./Dilon.db"
                },
                new Tenant
                {
                    TenantId = Guid.Parse("011dc86b-ecad-0f34-19e8-8e9644b780a3"),
                    Name = "其他租户",
                    Host = "localhost:5588",
                    CreatedTime = DateTime.Parse("2021-04-03 00:00:00"),
                    ConnectionString = "Data Source=./Dilon2.db"
                }
            };
        }
    }
}
