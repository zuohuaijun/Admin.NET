using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Linq.Expressions;
using Yitter.IdGenerator;

namespace Covid19.EntityFramework.Core
{
    [AppDbContext("DefaultConnection", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>, IMultiTenantOnTable, IModelBuilderFilter
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            // 启用实体数据更改监听
            EnabledEntityChangedListener = true;

            // 忽略空值更新
            InsertOrUpdateIgnoreNullValues = true;
        }

        /// <summary>
        /// 获取租户Id
        /// </summary>
        /// <returns></returns>
        public object GetTenantId()
        {
            if (App.User == null) return null;
            return Convert.ToInt64(App.User.FindFirst(ClaimConst.TENANT_ID)?.Value);
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
            // 配置租户Id以及假删除过滤器
            LambdaExpression expression = TenantIdAndFakeDeleteQueryFilterExpression(entityBuilder, dbContext);
            if (expression != null)
                entityBuilder.HasQueryFilter(expression);
        }

        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取当前事件对应上下文
            var dbContext = eventData.Context;
            // 获取所有更改，删除，新增的实体，但排除审计实体（避免死循环）
            var entities = dbContext.ChangeTracker.Entries()
                  .Where(u => u.Entity.GetType() != typeof(SysLogAudit) && u.Entity.GetType() != typeof(SysLogOp) &&
                              u.Entity.GetType() != typeof(SysLogVis) && u.Entity.GetType() != typeof(SysLogEx) &&
                        (u.State == EntityState.Modified || u.State == EntityState.Deleted || u.State == EntityState.Added)).ToList();
            if (entities == null || entities.Count < 1) return;

            // 判断是否是演示环境
            var demoEnvFlag = App.GetService<ISysConfigService>().GetDemoEnvFlag().GetAwaiter().GetResult();
            if (demoEnvFlag)
            {
                var sysUser = entities.Find(u => u.Entity.GetType() == typeof(SysUser));
                if (sysUser == null || string.IsNullOrEmpty((sysUser.Entity as SysUser).LastLoginTime.ToString())) // 排除登录
                    throw Oops.Oh(ErrorCode.D1200);
            }

            // 当前操作者信息
            var userId = App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
            var userName = App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;

            foreach (var entity in entities)
            {
                if (entity.Entity.GetType().IsSubclassOf(typeof(DBEntityTenant)))
                {
                    var obj = entity.Entity as DBEntityTenant;
                    switch (entity.State)
                    {
                        // 自动设置租户Id
                        case EntityState.Added:
                            var tenantId = entity.Property(nameof(Entity.TenantId)).CurrentValue;
                            if (tenantId == null || (long)tenantId == 0)
                                entity.Property(nameof(Entity.TenantId)).CurrentValue = long.Parse(GetTenantId().ToString());

                            obj.Id = obj.Id == 0 ? YitIdHelper.NextId() : obj.Id;
                            obj.CreatedTime = DateTimeOffset.Now;
                            if (!string.IsNullOrEmpty(userId))
                            {
                                obj.CreatedUserId = long.Parse(userId);
                                obj.CreatedUserName = userName;
                            }
                            break;
                        // 排除租户Id
                        case EntityState.Modified:
                            entity.Property(nameof(Entity.TenantId)).IsModified = false;
                            obj.UpdatedTime = DateTimeOffset.Now;
                            obj.UpdatedUserId = long.Parse(userId);
                            obj.UpdatedUserName = userName;
                            break;
                    }
                }
                else if (entity.Entity.GetType().IsSubclassOf(typeof(DEntityBase)))
                {
                    var obj = entity.Entity as DEntityBase;
                    if (entity.State == EntityState.Added)
                    {
                        obj.Id = obj.Id == 0 ? YitIdHelper.NextId() : obj.Id;
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
            }
        }

        /// <summary>
        /// 构建租户Id以及假删除过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="isDeletedKey"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        protected static LambdaExpression TenantIdAndFakeDeleteQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string onTableTenantId = null, string isDeletedKey = null, object filterValue = null)
        {
            onTableTenantId ??= "TenantId";
            isDeletedKey ??= "IsDeleted";
            IMutableEntityType metadata = entityBuilder.Metadata;
            if (metadata.FindProperty(onTableTenantId) == null || metadata.FindProperty(isDeletedKey) == null)
            {
                return null;
            }

            Expression finialExpression = Expression.Constant(true);
            ParameterExpression parameterExpression = Expression.Parameter(metadata.ClrType, "u");

            // 租户过滤器
            if (entityBuilder.Metadata.ClrType.BaseType.Name == typeof(DBEntityTenant).Name)
            {
                if (metadata.FindProperty(onTableTenantId) != null)
                {
                    ConstantExpression constantExpression = Expression.Constant(onTableTenantId);
                    MethodCallExpression right = Expression.Call(Expression.Constant(dbContext), dbContext.GetType().GetMethod("GetTenantId"));
                    finialExpression = Expression.AndAlso(finialExpression, Expression.Equal(Expression.Call(typeof(EF), "Property", new Type[1]
                    {
                        typeof(object)
                    }, parameterExpression, constantExpression), right));
                }
            }

            // 假删除过滤器
            if (metadata.FindProperty(isDeletedKey) != null)
            {
                ConstantExpression constantExpression = Expression.Constant(isDeletedKey);
                ConstantExpression right = Expression.Constant(filterValue ?? false);
                var fakeDeleteQueryExpression = Expression.Equal(Expression.Call(typeof(EF), "Property", new Type[1]
                {
                    typeof(bool)
                }, parameterExpression, constantExpression), right);
                finialExpression = Expression.AndAlso(finialExpression, fakeDeleteQueryExpression);
            }

            return Expression.Lambda(finialExpression, parameterExpression);
        }

        /// <summary>
        /// 配置假删除过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="isDeletedKey"></param>
        /// <returns></returns>
        protected LambdaExpression FakeDeleteQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string isDeletedKey = null)
        {
            return base.FakeDeleteQueryFilterExpression(entityBuilder, dbContext, isDeletedKey);
        }
    }
}