using Dilon.Core;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Dilon.EntityFramework.Core
{
    [AppDbContext("DefaultConnection", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>, IModelBuilderFilter
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            // 启用实体数据更改监听
            EnabledEntityChangedListener = true;
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
                throw Oops.Oh(ErrorCode.D1200);

            var userManager = App.GetService<IUserManager>();
            var userId = userManager?.UserId;

            // 获取所有已更改的实体
            foreach (var entity in entities)
            {
                if (entity.Entity.GetType().IsSubclassOf(typeof(DEntityBase)))
                {
                    var obj = entity.Entity as DEntityBase;
                    if (entity.State == EntityState.Added)
                    {
                        obj.CreatedTime = DateTimeOffset.Now;
                        obj.CreatedUserId = userId;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        obj.UpdatedTime = DateTimeOffset.Now;
                        obj.UpdatedUserId = userId;
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