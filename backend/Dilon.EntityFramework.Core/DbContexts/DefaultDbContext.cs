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

namespace Dilon.EntityFramework.Core
{
    [AppDbContext("DefaultConnection", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>, IMultiTenantOnDatabase, IModelBuilderFilter
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            // 启用实体数据更改监听
            EnabledEntityChangedListener = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(GetDatabaseConnectionString());

            base.OnConfiguring(optionsBuilder);
        }

        public string GetDatabaseConnectionString()
        {
            var defaultConnection = App.Configuration["ConnectionStrings:DefaultConnection"];

            // 如果没有实现多租户方式，则无需查询
            if (!typeof(IPrivateMultiTenant).IsAssignableFrom(GetType())) return defaultConnection;

            // 判断 HttpContext 是否存在
            var httpContext = App.HttpContext;
            if (httpContext == null) return defaultConnection;

            // 当前根据主机名称获取租户信息（可自由处理，比如请求参数等）
            var host = httpContext.Request.Host.Value;

            // 从分布式缓存中读取或查询数据库
            var tenantCachedKey = $"MULTI_TENANT:{host}";
            var distributedCache = App.GetService<IDistributedCache>();
            var cachedValue = distributedCache.GetString(tenantCachedKey);

            var jsonSerializerProvider = App.GetService<IJsonSerializerProvider>();
            SysTenant currentTenant;
            if (string.IsNullOrEmpty(cachedValue))
            {
                currentTenant = Db.GetDbContext<MultiTenantDbContextLocator>().Set<SysTenant>().FirstOrDefault(u => u.Host == host);
                if (currentTenant != null)
                    distributedCache.SetString(tenantCachedKey, jsonSerializerProvider.Serialize(currentTenant));
            }
            else currentTenant = jsonSerializerProvider.Deserialize<SysTenant>(cachedValue);
            return currentTenant?.Connection ?? defaultConnection;
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
            var expression = base.FakeDeleteQueryFilterExpression(entityBuilder, dbContext);
            if (expression == null) return;

            entityBuilder.HasQueryFilter(expression);
        }

        /// <summary>
        /// 重写实体保存之前
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
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

                //// 获取实体当前（现在）的值
                //var currentValues = entity.CurrentValues;
                //// 获取数据库中实体的值
                //var databaseValues = entity.GetDatabaseValues();

                //// 获取所有实体有效属性，排除 [NotMapper] 属性
                //var props = entity.OriginalValues.Properties;
                //foreach (var prop in props)
                //{
                //    var propName = prop.Name;  // 获取属性名                    
                //    var newValue = currentValues[propName];  // 获取现在的实体值

                //    object oldValue = null;
                //    // 如果是新增数据，则 databaseValues 为空，所以需要判断一下
                //    if (databaseValues != null)
                //        oldValue = databaseValues[propName];

                //    if ((newValue == oldValue) || (newValue != null && newValue.Equals(oldValue))) continue;
                //    // 插入审计日志表
                //    dbContext.AddAsync(new SysLogAudit
                //    {
                //        TableName = entity.Entity.GetType().Name,  // 获取实体类型（表名）
                //        ColumnName = propName,
                //        NewValue = newValue?.ToString(),
                //        OldValue = oldValue?.ToString(),
                //        CreatedTime = DateTime.Now,
                //        UserId = userId,
                //        UserName = userName,
                //        Operate = entity.State.ToString()  // 操作方式：新增、更新、删除
                //    });
                //}
            }
        }
    }
}