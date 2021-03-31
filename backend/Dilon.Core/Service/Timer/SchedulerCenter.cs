using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class SchedulerCenter : ISingleton
    {
        private IScheduler _scheduler = null;

        public SchedulerCenter()
        {
            _ = StartScheduleAsync();

            InitAllJob().GetAwaiter();
        }

        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        private async Task<bool> StartScheduleAsync()
        {
            if (_scheduler == null)
            {
                // 初始化Scheduler
                var schedulerFactory = new StdSchedulerFactory();
                _scheduler = await schedulerFactory.GetScheduler();

                // 开启调度器
                if (_scheduler.InStandbyMode)
                    await _scheduler.Start();
            }
            return _scheduler.InStandbyMode;
        }

        /// <summary>
        /// 停止调度器
        /// </summary>
        private async Task<bool> StopScheduleAsync()
        {
            //判断调度是否已经关闭
            if (!_scheduler.InStandbyMode)
            {
                //等待任务运行完成
                await _scheduler.Standby(); // 注意：Shutdown后Start会报错，所以这里使用暂停。
            }
            return !_scheduler.InStandbyMode;
        }

        /// <summary>
        /// 添加一个工作任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<dynamic> AddScheduleJobAsync(JobInput input)
        {
            // 检查任务是否已存在
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            if (await _scheduler.CheckExists(jobKey))
                throw Oops.Oh("任务已存在");

            // http请求配置
            var httpDir = new Dictionary<string, string>()
               {
                    { SchedulerDef.ENDAT, input.EndTime.ToString()},
                    { SchedulerDef.REQUESTURL, input.RequestUrl},
                    { SchedulerDef.HEADERS, input.Headers },
                    { SchedulerDef.REQUESTPARAMETERS, input.RequestParameters},
                    { SchedulerDef.REQUESTTYPE, ((int)input.RequestType).ToString()},
                    { SchedulerDef.RUNNUMBER, input.RunNumber.ToString()}
                };

            // 定义这个工作，并将其绑定到我们的IJob实现类                
            IJobDetail job = JobBuilder.Create<HttpJob>()
                             .SetJobData(new JobDataMap(httpDir))
                             .WithDescription(input.Remark)
                             .WithIdentity(input.JobName, input.JobGroup)
                             .Build();
            // 创建触发器
            ITrigger trigger = input.TriggerType == TriggerTypeEnum.Cron // && CronExpression.IsValidExpression(entity.Cron)
                               ? CreateCronTrigger(input)
                               : CreateSimpleTrigger(input);

            return await _scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task StopScheduleJobAsync(JobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            await _scheduler.PauseJob(jobKey);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteScheduleJobAsync(DeleteJobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            await _scheduler.PauseJob(jobKey);
            await _scheduler.DeleteJob(jobKey);
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimer/resumeJob")]
        public async Task<dynamic> ResumeJobAsync(JobInput input)
        {
            //检查任务是否存在
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            if (await _scheduler.CheckExists(jobKey))
            {
                var jobDetail = await _scheduler.GetJobDetail(jobKey);
                var endTime = jobDetail.JobDataMap.GetString(SchedulerDef.ENDAT);
                if (!string.IsNullOrWhiteSpace(endTime) && DateTime.Parse(endTime) <= DateTime.Now)
                {
                    throw Oops.Oh("Job的结束时间已过期");
                }
                else
                {
                    await _scheduler.ResumeJob(jobKey); // 任务已经存在则暂停任务
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<JobInput> QueryJobAsync(JobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            var jobDetail = await _scheduler.GetJobDetail(jobKey);
            var triggersList = await _scheduler.GetTriggersOfJob(jobKey);
            var triggers = triggersList.AsEnumerable().FirstOrDefault();
            var intervalSeconds = (triggers as SimpleTriggerImpl)?.RepeatInterval.TotalSeconds;
            var endTime = jobDetail.JobDataMap.GetString(SchedulerDef.ENDAT);
            return new JobInput
            {
                BeginTime = triggers.StartTimeUtc.LocalDateTime,
                EndTime = !string.IsNullOrWhiteSpace(endTime) ? DateTime.Parse(endTime) : null,
                Interval = intervalSeconds.HasValue ? Convert.ToInt32(intervalSeconds.Value) : null,
                JobGroup = input.JobGroup,
                JobName = input.JobName,
                Cron = (triggers as CronTriggerImpl)?.CronExpressionString,
                RunNumber = (triggers as SimpleTriggerImpl)?.RepeatCount,
                TriggerType = triggers is SimpleTriggerImpl ? TriggerTypeEnum.Simple : TriggerTypeEnum.Cron,
                Remark = jobDetail.Description,
                RequestUrl = jobDetail.JobDataMap.GetString(SchedulerDef.REQUESTURL),
                RequestType = (RequestTypeEnum)int.Parse(jobDetail.JobDataMap.GetString(SchedulerDef.REQUESTTYPE)),
                RequestParameters = jobDetail.JobDataMap.GetString(SchedulerDef.REQUESTPARAMETERS),
                Headers = jobDetail.JobDataMap.GetString(SchedulerDef.HEADERS)
            };
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task TriggerJobAsync(JobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            await _scheduler.ResumeJob(jobKey);
        }

        /// <summary>
        /// 获取任务日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<string>> GetJobLogsAsync(JobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            var jobDetail = await _scheduler.GetJobDetail(jobKey);
            return jobDetail.JobDataMap[SchedulerDef.LOGLIST] as List<string>;
        }

        /// <summary>
        /// 获取任务运行次数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<long> GetRunNumberAsync(JobInput input)
        {
            var jobKey = new JobKey(input.JobName, input.JobGroup);
            var jobDetail = await _scheduler.GetJobDetail(jobKey);
            return jobDetail.JobDataMap.GetLong(SchedulerDef.RUNNUMBER);
        }

        /// <summary>
        /// 获取所有任务详情
        /// </summary>
        /// <returns></returns>
        public async Task<List<JobOutput>> GetJobList()
        {
            var jobInfoList = new List<JobOutput>();
            var groupNames = await _scheduler.GetJobGroupNames();
            var jboKeyList = new List<JobKey>();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                //jobInfoList.Add(new JobOutput() { JobGroup = groupName });
            }
            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await _scheduler.GetJobDetail(jobKey);
                var triggersList = await _scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                var interval = triggers is SimpleTriggerImpl
                               ? ((triggers as SimpleTriggerImpl)?.RepeatInterval.ToString())
                               : ((triggers as CronTriggerImpl)?.CronExpressionString);

                jobInfoList.Add(new JobOutput
                {
                    JobName = jobKey.Name,
                    JobGroup = jobKey.Group,
                    LastErrMsg = jobDetail.JobDataMap.GetString(SchedulerDef.EXCEPTION),
                    RequestUrl = jobDetail.JobDataMap.GetString(SchedulerDef.REQUESTURL),
                    TriggerState = await _scheduler.GetTriggerState(triggers.Key),
                    PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                    NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                    BeginTime = triggers.StartTimeUtc.LocalDateTime,
                    Interval = interval,
                    EndTime = triggers.EndTimeUtc?.LocalDateTime,
                    Remark = jobDetail.Description,
                    RequestType = jobDetail.JobDataMap.GetString(SchedulerDef.REQUESTTYPE),
                    RunNumber = jobDetail.JobDataMap.GetString(SchedulerDef.RUNNUMBER)
                });
            }
            return jobInfoList;
        }

        /// <summary>
        /// 从数据库里面获取所有任务并初始化
        /// </summary>
        private async Task InitAllJob()
        {
            var jobList = Db.GetRepository<SysTimer>().DetachedEntities.Select(u => u.Adapt<JobInput>()).ToList();
            var jobTasks = new List<Task<dynamic>>();
            jobList.ForEach(u =>
            {
                jobTasks.Add(AddScheduleJobAsync(u));
            });
            await Task.WhenAll(jobTasks);
        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static ITrigger CreateSimpleTrigger(JobInput input)
        {
            //作业触发器
            if (input.RunNumber.HasValue && input.RunNumber > 0)
            {
                return TriggerBuilder.Create()
               .WithIdentity(input.JobName, input.JobGroup)
               .StartAt(input.BeginTime)//开始时间
                                        //.EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x =>
               {
                   x.WithIntervalInSeconds(input.Interval.Value)//执行时间间隔，单位秒
                        .WithRepeatCount(input.RunNumber.Value)//执行次数、默认从0开始
                        .WithMisfireHandlingInstructionFireNow();
               })
               .ForJob(input.JobName, input.JobGroup)//作业名称
               .Build();
            }
            else
            {
                return TriggerBuilder.Create()
               .WithIdentity(input.JobName, input.JobGroup)
               .StartAt(input.BeginTime)//开始时间
                                        //.EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x =>
               {
                   x.WithIntervalInSeconds(input.Interval.Value)//执行时间间隔，单位秒
                        .RepeatForever()//无限循环
                        .WithMisfireHandlingInstructionFireNow();
               })
               .ForJob(input.JobName, input.JobGroup)//作业名称
               .Build();
            }

        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static ITrigger CreateCronTrigger(JobInput input)
        {
            if (!CronExpression.IsValidExpression(input.Cron))
                throw Oops.Oh("Cron表达式错误");

            // 作业触发器
            return TriggerBuilder.Create()

                   .WithIdentity(input.JobName, input.JobGroup)
                   .StartAt(input.BeginTime) //开始时间
                                             //.EndAt(entity.EndTime) //结束时间
                   .WithCronSchedule(input.Cron, cronScheduleBuilder => cronScheduleBuilder.WithMisfireHandlingInstructionFireAndProceed())//指定cron表达式
                   .ForJob(input.JobName, input.JobGroup)//作业名称
                   .Build();
        }
    }
}
