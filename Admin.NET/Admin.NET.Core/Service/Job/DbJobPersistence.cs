namespace Admin.NET.Core.Service;

/// <summary>
/// 作业持久化（数据库）
/// </summary>
public class DbJobPersistence : IJobPersistence
{
    private readonly IServiceProvider _serviceProvider;

    public DbJobPersistence(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 作业调度服务启动时
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SchedulerBuilder> Preload()
    {
        // 扫描所有实现IJob的作业任务
        return App.EffectiveTypes.ScanToBuilders();
    }

    /// <summary>
    /// 作业计划初始化通知
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public SchedulerBuilder OnLoading(SchedulerBuilder builder)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var rep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysJobDetail>>();
        if (builder.Behavior == PersistenceBehavior.Removed)
        {
            rep.Delete(u => u.JobId == builder.GetJobBuilder().JobId);
            return builder.Removed();
        }
        if (rep.IsAny(u => u.JobId == builder.GetJobBuilder().JobId))
        {
            return builder.Updated();
        }

        return builder.Appended();
    }

    /// <summary>
    /// 作业计划Scheduler的JobDetail变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnChanged(PersistenceContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var db = serviceScope.ServiceProvider.GetService<ISqlSugarClient>();


        var jobDetail = context.JobDetail.Adapt<SysJobDetail>();
        if (context.Behavior == PersistenceBehavior.Appended)
        {
            db.Insertable(jobDetail).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Updated)
        {
            db.Updateable(jobDetail).WhereColumns(u => new { u.JobId }).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Removed)
        {
            db.Deleteable<SysJobDetail>().Where(u => u.JobId == jobDetail.JobId).ExecuteCommand();
        }
    }

    /// <summary>
    /// 作业计划Scheduler的触发器Trigger变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnTriggerChanged(PersistenceTriggerContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var db = serviceScope.ServiceProvider.GetService<ISqlSugarClient>();

        var jobTrigger = context.Trigger.Adapt<SysJobTrigger>();
        if (context.Behavior == PersistenceBehavior.Appended)
        {
            db.Insertable(jobTrigger).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Updated)
        {
            db.Updateable(jobTrigger).WhereColumns(u => new { u.TriggerId }).ExecuteCommand();
        }
        else if (context.Behavior == PersistenceBehavior.Removed)
        {
            db.Deleteable<SysJobTrigger>().Where(u => u.TriggerId == jobTrigger.TriggerId).ExecuteCommand();
        }
    }
}