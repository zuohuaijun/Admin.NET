using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Furion.RemoteRequest.Extensions;
using Furion.TaskScheduler;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务调度服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Timer", Order = 100)]
    public class SysTimerService : ISysTimerService, IDynamicApiController, IScoped
    {
        private readonly IRepository<SysTimer> _sysTimerRep;  // 任务表仓储
        private readonly ISysCacheService _cache;

        public SysTimerService(IRepository<SysTimer> sysTimerRep, ISysCacheService cache)
        {
            _sysTimerRep = sysTimerRep;
            _cache = cache;
        }

        /// <summary>
        /// 分页获取任务列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTimers/page")]
        public async Task<dynamic> GetTimerPageList([FromQuery] JobInput input)
        {
            var workers = SpareTime.GetWorkers().ToList();

            var timers = await _sysTimerRep.DetachedEntities
                                           .Where(!string.IsNullOrEmpty(input.JobName?.Trim()), u => EF.Functions.Like(u.JobName, $"%{input.JobName.Trim()}%"))
                                           .Select(u => u.Adapt<JobOutput>())
                                           .ToPagedListAsync(input.PageNo, input.PageSize);

            timers.Items.ToList().ForEach(u =>
            {
                var timer = workers.FirstOrDefault(m => m.WorkerName == u.JobName);
                if (timer != null)
                {
                    u.TimerStatus = timer.Status;
                    u.RunNumber = timer.Tally;
                    u.Exception = ""; // JSON.Serialize(timer.Exception);
                }
            });
            return XnPageResult<JobOutput>.PageResult(timers);
        }

        /// <summary>
        /// 获取所有本地任务
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysTimers/localJobList")]
        public async Task<dynamic> GetLocalJobList()
        {
            // 获取本地所有任务方法
            var LocalJobs = await GetTaskMethods();

            // TaskMethodInfo继承自LocalJobOutput，直接强转为LocalJobOutput再返回
            return LocalJobs.Select(t => (LocalJobOutput)t);
        }

        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/add")]
        public async Task AddTimer(JobInput input)
        {
            var isExist = await _sysTimerRep.AnyAsync(u => u.JobName == input.JobName, false);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1100);

            var timer = input.Adapt<SysTimer>();
            await _sysTimerRep.InsertAsync(timer);

            // 添加到任务调度里
            AddTimerJob(input);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/delete")]
        public async Task DeleteTimer(DeleteJobInput input)
        {
            var timer = await _sysTimerRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (timer == null)
                throw Oops.Oh(ErrorCode.D1101);

            await timer.DeleteAsync();

            // 从调度器里取消
            SpareTime.Cancel(timer.JobName);
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/edit")]
        public async Task UpdateTimber(UpdateJobInput input)
        {
            // 排除自己并且判断与其他是否相同
            var isExist = await _sysTimerRep.AnyAsync(u => u.JobName == input.JobName && u.Id != input.Id, false);
            if (isExist) throw Oops.Oh(ErrorCode.D1100);

            // 先从调度器里取消
            var oldTimer = await _sysTimerRep.FirstOrDefaultAsync(u => u.Id == input.Id, false);
            SpareTime.Cancel(oldTimer.JobName);

            var timer = input.Adapt<SysTimer>();
            await timer.UpdateAsync(ignoreNullValues: true);

            // 再添加到任务调度里
            AddTimerJob(input);
        }

        /// <summary>
        /// 查看任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTimers/detail")]
        public async Task<dynamic> GetTimer([FromQuery] QueryJobInput input)
        {
            return await _sysTimerRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/stop")]
        public void StopTimerJob(JobInput input)
        {
            SpareTime.Stop(input.JobName);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/start")]
        public void StartTimerJob(JobInput input)
        {
            var timer = SpareTime.GetWorkers().ToList().Find(u => u.WorkerName == input.JobName);
            if (timer == null)
                AddTimerJob(input);

            // 如果 StartNow 为 flase , 执行 AddTimerJob 并不会启动任务
            SpareTime.Start(input.JobName);
        }

        /// <summary>
        /// 新增定时任务
        /// </summary>
        /// <param name="input"></param>
        [NonAction]
        public void AddTimerJob(JobInput input)
        {
            Action<SpareTimer, long> action = null;

            switch (input.RequestType)
            {
                // 创建本地方法委托
                case RequestTypeEnum.Run:
                    {
                        // 查询符合条件的任务方法
                        var taskMethod = GetTaskMethods().Result.FirstOrDefault(m => m.RequestUrl == input.RequestUrl);
                        if (taskMethod == null) break;

                        // 创建任务对象
                        var typeInstance = Activator.CreateInstance(taskMethod.DeclaringType);

                        // 创建委托
                        action = (Action<SpareTimer, long>)Delegate.CreateDelegate(typeof(Action<SpareTimer, long>), typeInstance, taskMethod.MethodName);
                        break;
                    }
                // 创建网络任务委托
                default:
                    {
                        action = async (_, _) =>
                        {
                            var requestUrl = input.RequestUrl.Trim();
                            requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
                            var requestParameters = input.RequestParameters;
                            var headersString = input.Headers;
                            var headers = string.IsNullOrEmpty(headersString)
                                ? null
                                : JSON.Deserialize<Dictionary<string, string>>(headersString);

                            switch (input.RequestType)
                            {
                                case RequestTypeEnum.Get:
                                    await requestUrl.SetHeaders(headers).GetAsync();
                                    break;

                                case RequestTypeEnum.Post:
                                    await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PostAsync();
                                    break;

                                case RequestTypeEnum.Put:
                                    await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PutAsync();
                                    break;

                                case RequestTypeEnum.Delete:
                                    await requestUrl.SetHeaders(headers).DeleteAsync();
                                    break;
                            }
                        };
                        break;
                    }
            }

            if (action == null)
                throw Oops.Oh($"定时任务委托创建失败！JobName:{input.JobName}");

            // 缓存任务配置参数，以供任务运行时读取
            if (input.RequestType == RequestTypeEnum.Run)
            {
                var jobParametersName = $"{input.JobName}_Parameters";
                var jobParameters = _cache.Exists(jobParametersName);
                var requestParametersIsNull = string.IsNullOrEmpty(input.RequestParameters);

                // 如果没有任务配置却又存在缓存，则删除缓存
                if (requestParametersIsNull && jobParameters)
                    _cache.Del(jobParametersName);
                else if (!requestParametersIsNull)
                    _cache.Set(jobParametersName, JSON.Deserialize<Dictionary<string, string>>(input.RequestParameters));
            }

            // 创建定时任务
            switch (input.TimerType)
            {
                case SpareTimeTypes.Interval:
                    if (input.DoOnce)
                        SpareTime.DoOnce(input.Interval * 1000, action, input.JobName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                    else
                        SpareTime.Do(input.Interval * 1000, action, input.JobName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                    break;

                case SpareTimeTypes.Cron:
                    SpareTime.Do(input.Cron, action, input.JobName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                    break;
            }
        }

        /// <summary>
        /// 启动自启动任务
        /// </summary>
        [NonAction]
        public void StartTimerJob()
        {
            var sysTimerList = _sysTimerRep.DetachedEntities.Where(t => t.StartNow).Select(u => u.Adapt<JobInput>()).ToList();
            sysTimerList.ForEach(AddTimerJob);
        }

        /// <summary>
        /// 获取所有本地任务
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<IEnumerable<TaskMethodInfo>> GetTaskMethods()
        {
            // 有缓存就返回缓存
            var taskMethods = await _cache.GetAsync<IEnumerable<TaskMethodInfo>>("TaskMethodInfos");
            if (taskMethods != null) return taskMethods;

            // 获取所有本地任务方法，必须有spareTimeAttribute特性
            taskMethods = App.EffectiveTypes
                .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && typeof(ISpareTimeWorker).IsAssignableFrom(u))
                .SelectMany(u => u.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.IsDefined(typeof(SpareTimeAttribute), false) &&
                       m.GetParameters().Length == 2 &&
                       m.GetParameters()[0].ParameterType == typeof(SpareTimer) &&
                       m.GetParameters()[1].ParameterType == typeof(long) && m.ReturnType == typeof(void))
                .Select(m =>
                {
                    // 默认获取第一条任务特性
                    var spareTimeAttribute = m.GetCustomAttribute<SpareTimeAttribute>();
                    return new TaskMethodInfo
                    {
                        JobName = spareTimeAttribute.WorkerName,
                        RequestUrl = $"{m.DeclaringType.Name}/{m.Name}",
                        Cron = spareTimeAttribute.CronExpression,
                        DoOnce = spareTimeAttribute.DoOnce,
                        ExecuteType = spareTimeAttribute.ExecuteType,
                        Interval = (int)spareTimeAttribute.Interval / 1000,
                        StartNow = spareTimeAttribute.StartNow,
                        RequestType = RequestTypeEnum.Run,
                        Remark = spareTimeAttribute.Description,
                        TimerType = string.IsNullOrEmpty(spareTimeAttribute.CronExpression) ? SpareTimeTypes.Interval : SpareTimeTypes.Cron,
                        MethodName = m.Name,
                        DeclaringType = m.DeclaringType
                    };
                }));

            await _cache.SetAsync("TaskMethodInfos", taskMethods);
            return taskMethods;
        }
    }
}