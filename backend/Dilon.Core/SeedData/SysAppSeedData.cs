using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统应用表种子数据
    /// </summary>
    public class SysAppSeedData : IEntitySeedData<SysApp>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysApp> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysApp{Id=142307070898245, Name="开发平台", Code="system", Active="Y", Status=0, Sort=100 },
                new SysApp{Id=142307070922826, Name="运营平台", Code="advanced", Active="N", Status=0, Sort=200 },
                new SysApp{Id=142307070902341, Name="系统管理", Code="system_manage", Active="N", Status=0, Sort=300 },
                new SysApp{Id=142307070922869, Name="业务应用", Code="busapp", Active="N", Status=0, Sort=400 },
            };
        }
    }
}
