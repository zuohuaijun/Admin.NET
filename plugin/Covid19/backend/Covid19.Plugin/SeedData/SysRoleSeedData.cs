using Admin.NET.Core;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Covid19.Plugin
{
    /// <summary>
    /// 系统角色表种子数据
    /// </summary>
    public class SysRoleSeedData : IEntitySeedData<SysRole>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysRole> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysRole{Id=142307070922879, Name="核酸检测应用", Code="covid19_role", Sort=100, DataScopeType=DataScopeType.DEFINE, Remark="核酸检测应用", Status=0 }
            };
        }
    }
}
