using Dilon.Core;
using Dilon.Core.Entity;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Furion.Snowflake;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        /// <summary>
        /// 获取租户Id
        /// </summary>
        /// <returns></returns>
        public object GetTenantId()
        {
            if (App.User == null) return null;
            return App.User.FindFirst(ClaimConst.TENANT_ID)?.Value;
        }

        /// <summary>
        /// 配置租户Id过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            if (entityBuilder.Metadata.ClrType.BaseType == typeof(DBEntityTenant))
            {
                entityBuilder.HasQueryFilter(TenantIdQueryFilterExpression(entityBuilder, dbContext));
            }
        }

        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取当前事件对应上下文
            var dbContext = eventData.Context;
            // 获取所有更改，删除，新增的实体，但排除审计实体（避免死循环）
            var entities = dbContext.ChangeTracker.Entries()
                                    .Where(u => u.Entity.GetType() != typeof(SysLogAudit) && u.Entity.GetType() != typeof(SysLogOp) && u.Entity.GetType() != typeof(SysLogVis) &&
                                          (u.State == EntityState.Modified || u.State == EntityState.Deleted || u.State == EntityState.Added))
                                    .ToList();
            if (entities == null || entities.Count < 1) return;
            // 判断是否是演示环境
            var demoEnvFlag = App.GetService<ISysConfigService>().GetDemoEnvFlag().GetAwaiter().GetResult();
            if (demoEnvFlag)
            {
                var sysUser = entities.Find(u => u.Entity.GetType() == typeof(SysUser));
                if (sysUser == null || string.IsNullOrEmpty((sysUser.Entity as SysUser).LastLoginTime.ToString())) // 排除登录
                    throw Oops.Oh(ErrorCode.D1200);
            }

            // 当前操作用户信息
            var userId = App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
            var userName = App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;
            foreach (var entity in entities)
            {
                if (entity.Entity.GetType().IsSubclassOf(typeof(DEntityBase)))
                {
                    var obj = entity.Entity as DEntityBase;
                    if (entity.State == EntityState.Added)
                    {
                        obj.Id = IDGenerator.NextId();

                        obj.CreatedTime = DateTimeOffset.Now;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            obj.CreatedUserId = long.Parse(userId);
                            obj.CreatedUserName = userName;
                        }
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        obj.UpdatedTime = DateTimeOffset.Now;
                        obj.UpdatedUserId = long.Parse(userId);
                        obj.UpdatedUserName = userName;
                    }
                }
                else if (entity.Entity.GetType().IsSubclassOf(typeof(DBEntityTenant)))
                {
                    var obj = entity.Entity as DBEntityTenant;
                    switch (entity.State)
                    {                     
                        // 自动设置租户Id
                        case EntityState.Added:
                            var identityId = entity.Property(nameof(Entity.TenantId)).CurrentValue;
                            if (identityId == null || (long)identityId == 0)
                            {
                                entity.Property(nameof(Entity.TenantId)).CurrentValue = long.Parse(GetTenantId().ToString());
                            }                           
                            break;
                        // 排除租户Id
                        case EntityState.Modified:
                            entity.Property(nameof(Entity.TenantId)).IsModified = false;                          
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 配置租户Id过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="onTableTenantId"></param>
        /// <returns></returns>
        protected override LambdaExpression TenantIdQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string onTableTenantId = null)
        {
            LambdaExpression expression = base.TenantIdQueryFilterExpression(entityBuilder, dbContext, onTableTenantId);
            return expression;
        }

        /// <summary>
        /// 配置假删除过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="isDeletedKey"></param>
        /// <returns></returns>
        protected override LambdaExpression FakeDeleteQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string isDeletedKey = null)
        {
            return base.FakeDeleteQueryFilterExpression(entityBuilder, dbContext, isDeletedKey);
        }
    }
}