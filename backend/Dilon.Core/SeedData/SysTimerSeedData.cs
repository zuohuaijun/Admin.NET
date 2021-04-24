using Furion.DatabaseAccessor;
using Furion.TaskScheduler;
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
                new SysTimer
                {
                    Id = 142307070910556, JobName = "百度api", DoOnce = false, StartNow = false, Interval = 5,
                    TimerType = SpareTimeTypes.Interval, ExecuteType = SpareTimeExecuteTypes.Serial,
                    RequestUrl = "https://www.baidu.com", RequestType = RequestTypeEnum.Post, IsDeleted = false
                },
                new SysTimer
                {
                    Id = 142307070910557, JobName = "LogExWritingService", DoOnce = false, StartNow = true, Interval = 10,
                    TimerType = SpareTimeTypes.Interval, ExecuteType = SpareTimeExecuteTypes.Serial,
                    RequestUrl = "LogJobWorker/DoLogEx", RequestType = RequestTypeEnum.Run, IsDeleted = false,
                    Remark = "后台批量写异常日志，配置项参数：{\"quantity\": 2}，不填默认为2"
                },
                new SysTimer
                {
                    Id = 142307070910558, JobName = "LogOpWritingService", DoOnce = false, StartNow = true, Interval = 5,
                    TimerType = SpareTimeTypes.Interval, ExecuteType = SpareTimeExecuteTypes.Serial,
                    RequestUrl = "LogJobWorker/DoLogOp", RequestType = RequestTypeEnum.Run, IsDeleted = false,
                    Remark = "后台批量写操作日志，配置项参数：{\"quantity\": 2}，不填默认为2"
                },
                new SysTimer
                {
                    Id = 142307070910559, JobName = "LogVisWritingService", DoOnce = false, StartNow = true, Interval = 8,
                    TimerType = SpareTimeTypes.Interval, ExecuteType = SpareTimeExecuteTypes.Serial,
                    RequestUrl = "LogJobWorker/DoLogVis", RequestType = RequestTypeEnum.Run, IsDeleted = false,
                    Remark = "后台批量写访问日志，配置项参数：{\"quantity\": 2}，不填默认为2"
                },
            };
        }
    }
}
