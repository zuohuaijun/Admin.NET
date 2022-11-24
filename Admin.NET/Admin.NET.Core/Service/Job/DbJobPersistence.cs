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