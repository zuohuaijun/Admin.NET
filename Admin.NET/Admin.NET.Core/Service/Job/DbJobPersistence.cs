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
    /// 作业调度器服务启动时
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SchedulerBuilder> Preload()
    {
        // 扫描所有实现IJob的作业任务
        return App.EffectiveTypes.Where(t => t.IsJobType())
            .Select(t => SchedulerBuilder.Create(JobBuilder.Create(t), t.ScanTriggers()));
    }

    /// <summary>
    /// 作业计划加载完成（通常用来同步存储介质（如数据库）数据到内存中）
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    public SchedulerBuilder OnLoaded(string jobId, SchedulerBuilder builder)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var rep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysJobDetail>>();
        //if (builder.Behavior == PersistenceBehavior.Removed)
        //{
        //    rep.Delete(u => u.JobId == jobId);
        //    return builder.Removed();
        //}
        if (rep.IsAny(u => u.JobId == jobId))
        {
            return builder.Updated();
        }
        else
        {
            return builder.Appended();
        }
    }

    /// <summary>
    /// 作业计划Scheduler的JobDetail变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnChanged(PersistenceContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var db = serviceScope.ServiceProvider.GetService<ISqlSugarClient>();
        var sql = context.ConvertToSQL(db.EntityMaintenance.GetEntityInfo<SysJobDetail>().DbTableName, NamingConventions.Pascal);
        db.Ado.ExecuteCommand(sql);
    }

    /// <summary>
    /// 作业计划Scheduler的触发器Trigger变化时
    /// </summary>
    /// <param name="context"></param>
    public void OnTriggerChanged(PersistenceTriggerContext context)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var db = serviceScope.ServiceProvider.GetService<ISqlSugarClient>();
        var sql = context.ConvertToSQL(db.EntityMaintenance.GetEntityInfo<SysJobTrigger>().DbTableName, NamingConventions.Pascal);
        db.Ado.ExecuteCommand(sql);
    }
}