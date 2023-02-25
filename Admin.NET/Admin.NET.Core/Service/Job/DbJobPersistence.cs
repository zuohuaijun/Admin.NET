namespace Admin.NET.Core.Service;

/// <summary>
/// 作业持久化（数据库）
/// </summary>
public class DbJobPersistence : IJobPersistence
{
    private readonly IServiceScope _serviceScope;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly SqlSugarRepository<SysJobDetail> _jobRepository;
    private readonly SqlSugarRepository<SysJobTrigger> _triggerRepository;
    private readonly DynamicJobCompiler _dynamicJobCompiler;

    public DbJobPersistence(IServiceProvider serviceProvider, DynamicJobCompiler dynamicJobCompiler)
    {
        _serviceScope = serviceProvider.CreateScope();
        var services = _serviceScope.ServiceProvider;
        _dynamicJobCompiler = dynamicJobCompiler;
        _sqlSugarClient = services.GetService<ISqlSugarClient>();
        _jobRepository = services.GetService<SqlSugarRepository<SysJobDetail>>();
        _triggerRepository = services.GetService<SqlSugarRepository<SysJobTrigger>>();
    }

    /// <summary>
    /// 作业调度服务启动时
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SchedulerBuilder> Preload()
    {

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
        var jobDetail = context.JobDetail.Adapt<SysJobDetail>();
        if (context.Behavior == PersistenceBehavior.Appended)
        {
            _sqlSugarClient.Insertable(jobDetail).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Updated)
        {
            _sqlSugarClient.Updateable(jobDetail).WhereColumns(u => new { u.JobId }).IgnoreColumns(u => new { u.Id }).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Removed)
        {
            _sqlSugarClient.Deleteable<SysJobDetail>().Where(u => u.JobId == jobDetail.JobId).ExecuteCommand();
        }
    }

    /// <summary>
    /// 作业计划Scheduler的触发器Trigger变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnTriggerChanged(PersistenceTriggerContext context)
    {
        var jobTrigger = context.Trigger.Adapt<SysJobTrigger>();
        if (context.Behavior == PersistenceBehavior.Appended)
        {
            _sqlSugarClient.Insertable(jobTrigger).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Updated)
        {
            _sqlSugarClient.Updateable(jobTrigger).WhereColumns(u => new { u.TriggerId, u.JobId }).IgnoreColumns(u => new { u.Id }).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Removed)
        {
            _sqlSugarClient.Deleteable<SysJobTrigger>().Where(u => u.TriggerId == jobTrigger.TriggerId && u.JobId == jobTrigger.JobId).ExecuteCommand();
        }
    }
   
    public void Dispose()
    {
        _serviceScope?.Dispose();
    }
}