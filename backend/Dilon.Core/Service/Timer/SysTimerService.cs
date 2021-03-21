using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务调度服务
    /// </summary>
    [ApiDescriptionSettings(Name = "sysTimer", Order = 100)]
    public class SysTimerService : ISysTimerService, IDynamicApiController, IScoped
    {
        private readonly IRepository<SysTimer> _sysTimerRep;  // 任务表仓储 
        private readonly SchedulerCenter _schedulerCenter;

        public SysTimerService(IRepository<SysTimer> sysTimerRep, SchedulerCenter schedulerCenter)
        {
            _sysTimerRep = sysTimerRep;
            _schedulerCenter = schedulerCenter;
        }

        /// <summary>
        /// 分页获取任务列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTimers/page")]
        public async Task<dynamic> GetJobPageList([FromQuery] JobInput input)
        {
            var jobList = await _schedulerCenter.GetJobList();

            var jobName = !string.IsNullOrEmpty(input.JobName?.Trim());
            var timers = await _sysTimerRep.DetachedEntities
                                  .Where((jobName, u => EF.Functions.Like(u.JobName, $"%{input.JobName.Trim()}%")))
                                  .Select(u => u.Adapt<JobInput>())
                                  .ToPagedListAsync(input.PageNo, input.PageSize);
            timers.Items.ToList().ForEach(u =>
            {
                u.DisplayState = jobList.Find(m => m.JobName == u.JobName)?.DisplayState;
            });
            return XnPageResult<JobInput>.PageResult(timers);
        }

        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/add")]
        public async Task AddJob(JobInput input)
        {
            var isExist = await _sysTimerRep.AnyAsync(u => u.JobName == input.JobName, false);
            if (isExist)
                throw Oops.Oh(ErrorCode.D1100);

            var timer = input.Adapt<SysTimer>();
            await _sysTimerRep.InsertAsync(timer);

            // 添加到调度
            await _schedulerCenter.AddScheduleJobAsync(input);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/delete")]
        public async Task DeleteJob(DeleteJobInput input)
        {
            var timer = await _sysTimerRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (timer == null)
                throw Oops.Oh(ErrorCode.D1101);

            await timer.DeleteNowAsync();

            // 从调度器里删除
            await _schedulerCenter.DeleteScheduleJobAsync(input);
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/edit")]
        public async Task UpdateJob(UpdateJobInput input)
        {
            // 排除自己并且判断与其他是否相同
            var isExist = await _sysTimerRep.AnyAsync(u => u.JobName == input.JobName && u.Id != input.Id, false);
            if (isExist) throw Oops.Oh(ErrorCode.D1100);

            var timer = input.Adapt<SysTimer>();
            await timer.UpdateNowAsync(ignoreNullValues: true);

            // 先从调度器里删除
            var oldTimer = await _sysTimerRep.FirstOrDefaultAsync(u => u.Id == input.Id, false);
            await _schedulerCenter.DeleteScheduleJobAsync(oldTimer.Adapt<DeleteJobInput>());

            // 再加到调度里
            await _schedulerCenter.AddScheduleJobAsync(timer.Adapt<JobInput>());
        }

        /// <summary>
        /// 查看任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysTimers/detail")]
        public async Task<dynamic> GetTimer([FromQuery] JobInput input)
        {
            return await _sysTimerRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/stop")]
        public async Task StopScheduleJobAsync(JobInput input)
        {
            await _schedulerCenter.StopScheduleJobAsync(input);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysTimers/start")]
        public async Task TriggerJobAsync(JobInput input)
        {
            await _schedulerCenter.TriggerJobAsync(input);
        }
    }
}
