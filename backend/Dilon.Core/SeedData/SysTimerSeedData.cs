using Dilon.Core.Service;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统任务调度表种子数据
    /// </summary>
    public class SysTimerSeedData : IEntitySeedData<SysTimer>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysTimer> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysTimer{Id=1, JobName="百度api", JobGroup="默认分组", BeginTime=DateTimeOffset.Parse("2021-03-21 00:00:00+08:00"), Interval=5, TriggerType=TriggerTypeEnum.Simple, RequestUrl="https://www.baidu.com", RequestType=RequestTypeEnum.Post, IsDeleted=false },
            };
        }
    }
}
