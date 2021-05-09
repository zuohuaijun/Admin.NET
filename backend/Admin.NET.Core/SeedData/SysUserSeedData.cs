using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core
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
                new SysUser{TenantId=142307070918780, Id=142307070910551, Account="superAdmin", Name="超级管理员", NickName="superAdmin", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.SuperAdmin, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918780, Id=142307070910552, Account="admin", Name="系统管理员", NickName="admin", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.Admin, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918780, Id=142307070910553, Account="zuohuaijun", Name="普通用户", NickName="zuohuaijun", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },

                new SysUser{TenantId=142307070918781, Id=142307070910554, Account="zuohuaijun@163.com", Name="系统管理员", NickName="admin", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.Admin, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
                new SysUser{TenantId=142307070918781, Id=142307070910556, Account="dilon@163.com", Name="普通用户", NickName="dilon", Password="e10adc3949ba59abbe56e057f20f883e", AdminType=AdminType.None, Birthday=DateTimeOffset.Parse("1986-07-26 00:00:00"), Phone="18020030720", Sex=Gender.MALE, IsDeleted=false },
            };
        }
    }
}