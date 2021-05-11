using Admin.NET.Core;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Covid19.Plugin
{
    /// <summary>
    /// 系统职位表种子数据
    /// </summary>
    public class SysPosSeedData : IEntitySeedData<SysPos>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysPos{Id=142307070926918, Name="样本采集员", Code="xg_cjy", Sort=100, Remark="样本采集员", Status=0 },
                new SysPos{Id=142307070926919, Name="核酸检验员", Code="xg_jyy", Sort=100, Remark="核酸检验员", Status=0 },
                new SysPos{Id=142307070926920, Name="数据审核员", Code="xg_shy", Sort=100, Remark="数据审核员", Status=0 },
            };
        }
    }
}
