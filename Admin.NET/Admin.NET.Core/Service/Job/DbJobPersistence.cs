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

    /// <summary>
    /// 作业调度器服务启动时（通常用来同步持久化数据到内存中）
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    public SchedulerBuilder Preload(string jobId, SchedulerBuilder builder)
    {
        // 如果是更新操作，则 return builder.Updated();
        // 如果是新增操作，则 return builder.Appended();
        // 如果是删除操作，则 return builder.Removed();
        return builder.Updated();
    }
}