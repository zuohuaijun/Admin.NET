using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using Yitter.IdGenerator;

namespace Covid19.EntityFramework.Core
{
    [AppDbContext("MultiTenantConnection", DbProvider.Sqlite)]
    public class MultiTenantDbContext : AppDbContext<MultiTenantDbContext, MultiTenantDbContextLocator>
    {
        public MultiTenantDbContext(DbContextOptions<MultiTenantDbContext> options) : base(options)
        {
        }

        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取所有已更改的实体
            var entities = eventData.Context.ChangeTracker.Entries()
                                    .Where(u => u.State == EntityState.Modified || u.State == EntityState.Deleted || u.State == EntityState.Added)
                                    .ToList();

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
                        obj.Id = YitIdHelper.NextId();
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
    }
}