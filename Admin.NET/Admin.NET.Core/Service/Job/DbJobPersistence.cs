namespace Admin.NET.Core.Service;

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
        var jobRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobDetail>>();
        var triggerRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobTrigger>>();
        var dynamicJobCompiler = scope.ServiceProvider.GetRequiredService<DynamicJobCompiler>();
        // 获取所有定义的作业
        var allJobs = App.EffectiveTypes.ScanToBuilders().ToList();
        // 若数据库不存在任何作业，则直接返回
        if (!jobRepository.IsAny(u => true)) return allJobs;

        // 遍历所有定义的作业
        foreach (var schedulerBuilder in allJobs)
        {
            // 获取作业信息构建器
            var jobBuilder = schedulerBuilder.GetJobBuilder();

            // 加载数据库数据
            var dbDetail = jobRepository.GetFirst(u => u.JobId == jobBuilder.JobId);
            if (dbDetail == null) continue;

            // 同步数据库数据
            jobBuilder.LoadFrom(dbDetail);

            // 获取作业的所有数据库的触发器
            var dbTriggers = triggerRepository.GetList(u => u.JobId == jobBuilder.JobId).ToArray();
            // 遍历所有作业触发器
            foreach (var (_, triggerBuilder) in schedulerBuilder.GetEnumerable())
            {
                // 加载数据库数据
                var dbTrigger = dbTriggers.FirstOrDefault(u => u.JobId == jobBuilder.JobId && u.TriggerId == triggerBuilder.TriggerId);
                if (dbTrigger == null) continue;

                triggerBuilder.LoadFrom(dbTrigger).Updated(); // 标记更新
            }
            // 遍历所有非编译时定义的触发器加入到作业中
            foreach (var dbTrigger in dbTriggers)
            {
                if (schedulerBuilder.GetTriggerBuilder(dbTrigger.TriggerId)?.JobId == jobBuilder.JobId) continue;
                var triggerBuilder = TriggerBuilder.Create(dbTrigger.TriggerId).LoadFrom(dbTrigger);
                schedulerBuilder.AddTriggerBuilder(triggerBuilder); // 先添加
                triggerBuilder.Updated(); // 再标记更新
            }

            // 标记更新
            schedulerBuilder.Updated();
        }

        // 获取数据库所有通过脚本创建的作业
        var allDbScriptJobs = jobRepository.GetList(u => u.CreateType != JobCreateTypeEnum.BuiltIn);
        foreach (var dbDetail in allDbScriptJobs)
        {
            // 动态创建作业
            Type jobType;
            switch (dbDetail.CreateType)
            {
                case JobCreateTypeEnum.Script:
                    jobType = dynamicJobCompiler.BuildJob(dbDetail.ScriptCode);
                    break;
                case JobCreateTypeEnum.Http:
                    jobType = typeof(HttpJob);
                    break;
                default:
                    throw new NotSupportedException();
            }

            var jobBuilder = JobBuilder.Create(jobType).LoadFrom(dbDetail);

            // 强行设置为不扫描 IJob 实现类 [Trigger] 特性触发器，否则 SchedulerBuilder.Create 会再次扫描，导致重复添加同名触发器
            jobBuilder.SetIncludeAnnotations(false);

            // 获取作业的所有数据库的触发器加入到作业中
            var dbTriggers = triggerRepository.GetList(u => u.JobId == jobBuilder.JobId).ToArray();
            var triggerBuilders = dbTriggers.Select(u => TriggerBuilder.Create(u.TriggerId).LoadFrom(u).Updated());
            var schedulerBuilder = SchedulerBuilder.Create(jobBuilder, triggerBuilders.ToArray());

            // 标记更新
            schedulerBuilder.Updated();

            allJobs.Add(schedulerBuilder);
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
        var jobRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobDetail>>();

        var jobDetail = context.JobDetail.Adapt<SysJobDetail>();
        switch (context.Behavior)
        {
            case PersistenceBehavior.Appended:
                jobRepository.AsInsertable(jobDetail).ExecuteCommand();
                break;

            case PersistenceBehavior.Updated:
                jobRepository.AsUpdateable(jobDetail).WhereColumns(u => new { u.JobId }).IgnoreColumns(u => new { u.Id, u.CreateType, u.ScriptCode }).ExecuteCommand();
                break;

            case PersistenceBehavior.Removed:
                jobRepository.AsDeleteable().Where(u => u.JobId == jobDetail.JobId).ExecuteCommand();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// 作业计划Scheduler的触发器Trigger变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnTriggerChanged(PersistenceTriggerContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var triggerRepository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysJobTrigger>>();
        var jobTrigger = context.Trigger.Adapt<SysJobTrigger>();
        switch (context.Behavior)
        {
            case PersistenceBehavior.Appended:
                triggerRepository.AsInsertable(jobTrigger).ExecuteCommand();
                break;

            case PersistenceBehavior.Updated:
                triggerRepository.AsUpdateable(jobTrigger).WhereColumns(u => new { u.TriggerId, u.JobId }).IgnoreColumns(u => new { u.Id }).ExecuteCommand();
                break;

            case PersistenceBehavior.Removed:
                triggerRepository.AsDeleteable().Where(u => u.TriggerId == jobTrigger.TriggerId && u.JobId == jobTrigger.JobId).ExecuteCommand();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}