using Dilon.Core;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Furion.Snowflake;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dilon.EntityFramework.Core
{
    [AppDbContext("DefaultConnection", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>, IMultiTenantOnTable, IModelBuilderFilter
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            // 启用实体数据更改监听
            EnabledEntityChangedListener = true;
        }

        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取当前事件对应上下文
            var dbContext = eventData.Context;

            // 获取所有新增和更新的实体
            var entities = dbContext.ChangeTracker.Entries().Where(u => u.State == EntityState.Added || u.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                if (entity.Metadata.ClrType.BaseType.Name != "DBEntityTenant") {
                    continue;
                }
                switch (entity.State)
                {
                    // 自动设置租户Id
                    case EntityState.Added:
                        entity.Property(nameof(Entity.TenantId)).CurrentValue =long.Parse( GetTenantId().ToString());
                        break;
                    // 排除租户Id
                    case EntityState.Modified:
                        entity.Property(nameof(Entity.TenantId)).IsModified = false;
                        break;
                }
            }
        }

        protected override LambdaExpression TenantIdQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string onTableTenantId = null)
        {
            LambdaExpression expression = base.TenantIdQueryFilterExpression(entityBuilder, dbContext, onTableTenantId);
            return expression;
        }



        /// <summary>
        /// 配置假删除过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            

            if (entityBuilder.Metadata.ClrType.BaseType.Name != "DBEntityTenant")
            {
                return;
            }
            object tenantId =GetTenantId();
            if (tenantId == null)
            {
                return;
            }
            //var expression = base.FakeDeleteQueryFilterExpression(entityBuilder, dbContext);
            //if (expression == null) return;

            //entityBuilder.HasQueryFilter(expression);
            entityBuilder.HasQueryFilter(TenantIdQueryFilterExpression(entityBuilder, dbContext));

        }



        public object GetTenantId()
        {
            var tenantId = App.User.FindFirst(ClaimConst.TENANT_ID)?.Value;
            return tenantId;
        }
    }
}