using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统用户表种子数据
    /// </summary>
    public class SysUserSeedData : IEntitySeedData<SysUser>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysUser> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysUser{TenantId=142307070918780, Id=142307070910551, Account="superAdmin", Name="superAdmin", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.SuperAdmin, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918780, Id=142307070910552, Account="admin", Name="admin", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.SuperAdmin, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918780, Id=142307070910553, Account="zuohuaijun", Name="zuohuaijun", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },

                new SysUser{TenantId=142307070918781, Id=142307070910554, Account="admin1", Name="admin1", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918781, Id=142307070910556, Account="zuohuaijun1", Name="zuohuaijun1", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918782, Id=142307070910557, Account="admin2", Name="admin2", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918782, Id=142307070910559, Account="zuohuaijun2", Name="zuohuaijun2", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false }
            };
        }
    }
}
