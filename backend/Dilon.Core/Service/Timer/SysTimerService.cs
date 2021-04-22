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
using System.Collections.Generic;
using System.Linq;
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

        public SysTimerService(IRepository<SysTimer> sysTimerRep)
        {
            _sysTimerRep = sysTimerRep;
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

            var jobName = !string.IsNullOrEmpty(input.JobName?.Trim());
            var timers = await _sysTimerRep.DetachedEntities
                                  .Where((jobName, u => EF.Functions.Like(u.JobName, $"%{input.JobName.Trim()}%")))
                                  .Select(u => u.Adapt<JobOutput>())
                                  .ToPagedListAsync(input.PageNo, input.PageSize);

            timers.Items.ToList().ForEach(u =>
            {
                var timer = workers.FirstOrDefault(m => m.WorkerName == u.JobName);
                if (timer != null)
                {
                    u.TimerStatus = timer.Status;
                    u.Exception = ""; // JSON.Serialize(timer.Exception);
                }
            });
            return XnPageResult<JobOutput>.PageResult(timers);
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
            else
                SpareTime.Start(input.JobName);
        }

        /// <summary>
        /// 新增定时任务
        /// </summary>
        /// <param name="input"></param>
        [NonAction]
        public void AddTimerJob(JobInput input)
        {
            var timerType = input.TimerType == SpareTimeTypes.Interval ? (input.Interval * 1000).ToString() : input.Cron;
            SpareTime.Do(timerType, async (timer, count) =>
            {
                //if (timer.Exception.Any())
                //    throw Oops.Oh(timer.Exception.Values.LastOrDefault()?.Message);

                var requestUrl = input.RequestUrl.Trim();
                requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
                var requestParameters = input.RequestParameters;
                var headersString = input.Headers;
                var headers = string.IsNullOrEmpty(headersString) ? null : JSON.Deserialize<Dictionary<string, string>>(headersString);
                var requestType = input.RequestType;
                switch (requestType)
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
            }, input.JobName, input.Remark, startNow: true);
        }
    }
}
