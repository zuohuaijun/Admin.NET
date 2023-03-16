using Admin.NET.Core;
using ConferenceAIOServer.Core;
using Furion;
using Furion.Schedule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace ConferenceAIOServer.Core
{

    /// <summary>
    /// 作业持久化（数据库）
    /// </summary>
    public class DbJobPersistence : IJobPersistence
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public DbJobPersistence(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 作业调度服务启动时
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SchedulerBuilder> Preload()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _jobRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobDetail>>();
            var _triggerRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobTrigger>>();
            // 获取所有定义的作业
            var allJobs = App.EffectiveTypes.ScanToBuilders();
            // 若数据库不存在任何作业，则直接返回
            if (!_jobRepository.IsAny(u => true)) return allJobs;

            // 遍历所有定义的作业
            foreach (var schedulerBuilder in allJobs)
            {
                // 获取作业信息构建器
                var jobBuilder = schedulerBuilder.GetJobBuilder();

                // 加载数据库数据
                var dbDetail = _jobRepository.GetFirst(u => u.JobId == jobBuilder.JobId);
                if (dbDetail == null) continue;

                // 同步数据库数据
                jobBuilder.LoadFrom(dbDetail);

                // 遍历所有作业触发器
                foreach (var (_, triggerBuilder) in schedulerBuilder.GetEnumerable())
                {
                    // 加载数据库数据
                    var dbTrigger = _triggerRepository.GetFirst(u => u.JobId == jobBuilder.JobId && u.TriggerId == triggerBuilder.TriggerId);
                    if (dbTrigger == null) continue;

                    triggerBuilder.LoadFrom(dbTrigger)
                                  .Updated();   // 标记更新
                }

                // 标记更新
                schedulerBuilder.Updated();
            }

            return allJobs;
        }

        /// <summary>
        /// 作业计划初始化通知
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public SchedulerBuilder OnLoading(SchedulerBuilder builder)
        {
            return builder;
        }

        /// <summary>
        /// 作业计划Scheduler的JobDetail变化时
        /// </summary>
        /// <param name="context"></param>
        public void OnChanged(PersistenceContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _jobRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobDetail>>();

            var jobDetail = context.JobDetail.Adapt<SysJobDetail>();
            if (context.Behavior == PersistenceBehavior.Appended)
            {
                _jobRepository.AsInsertable(jobDetail).ExecuteCommand();
            }
            else if (context.Behavior == PersistenceBehavior.Updated)
            {
                _jobRepository.AsUpdateable(jobDetail).WhereColumns(u => new { u.JobId }).IgnoreColumns(u => new { u.Id }).ExecuteCommand();
            }
            else if (context.Behavior == PersistenceBehavior.Removed)
            {
                _jobRepository.AsDeleteable().Where(u => u.JobId == jobDetail.JobId).ExecuteCommand();
            }
        }

        /// <summary>
        /// 作业计划Scheduler的触发器Trigger变化时
        /// </summary>
        /// <param name="context"></param>
        public void OnTriggerChanged(PersistenceTriggerContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _triggerRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobTrigger>>();
            var jobTrigger = context.Trigger.Adapt<SysJobTrigger>();
            if (context.Behavior == PersistenceBehavior.Appended)
            {
                _triggerRepository.AsInsertable(jobTrigger).ExecuteCommand();
            }
            else if (context.Behavior == PersistenceBehavior.Updated)
            {
                _triggerRepository.AsUpdateable(jobTrigger).WhereColumns(u => new { u.TriggerId, u.JobId }).IgnoreColumns(u => new { u.Id }).ExecuteCommand();
            }
            else if (context.Behavior == PersistenceBehavior.Removed)
            {
                _triggerRepository.AsDeleteable().Where(u => u.TriggerId == jobTrigger.TriggerId && u.JobId == jobTrigger.JobId).ExecuteCommand();
            }
        }

    }
}