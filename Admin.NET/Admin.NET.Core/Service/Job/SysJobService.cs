namespace Admin.NET.Core.Service;

/// <summary>
/// 系统作业任务服务
/// </summary>
[ApiDescriptionSettings(Order = 320)]
public class SysJobService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysJobDetail> _sysJobDetailRep;
    private readonly SqlSugarRepository<SysJobTrigger> _sysJobTriggerRep;
    private readonly SqlSugarRepository<SysJobCluster> _sysJobClusterRep;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly DynamicJobCompiler _dynamicJobCompiler;

    public SysJobService(SqlSugarRepository<SysJobDetail> sysJobDetailRep,
        SqlSugarRepository<SysJobTrigger> sysJobTriggerRep,
        SqlSugarRepository<SysJobCluster> sysJobClusterRep,
        ISchedulerFactory schedulerFactory,
        DynamicJobCompiler dynamicJobCompiler)
    {
        _sysJobDetailRep = sysJobDetailRep;
        _sysJobTriggerRep = sysJobTriggerRep;
        _sysJobClusterRep = sysJobClusterRep;
        _schedulerFactory = schedulerFactory;
        _dynamicJobCompiler = dynamicJobCompiler;
    }

    /// <summary>
    /// 获取作业分页列表
    /// </summary>
    [ApiDescriptionSettings(Name = "PageJobDetail")]
    [DisplayName("获取作业分页列表")]
    public async Task<SqlSugarPagedList<JobOutput>> GetPageJobDetail([FromQuery] PageJobInput input)
    {
        var jobDetails = await _sysJobDetailRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobId), u => u.JobId.Contains(input.JobId))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description))
            .Select(d => new JobOutput
            {
                JobDetail = d,
            }).ToPagedListAsync(input.Page, input.PageSize);
        await _sysJobDetailRep.AsSugarClient().ThenMapperAsync(jobDetails.Items, async u =>
        {
            u.JobTriggers = await _sysJobTriggerRep.GetListAsync(t => t.JobId == u.JobDetail.JobId);
        });

        // 提取中括号里面的参数值
        var rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");
        foreach (var job in jobDetails.Items)
        {
            foreach (var jobTrigger in job.JobTriggers)
            {
                jobTrigger.Args = rgx.Match(jobTrigger.Args ?? "").Value;
            }
        }
        return jobDetails;
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "AddJobDetail")]
    [DisplayName("添加作业")]
    public async Task AddJobDetail(AddJobDetailInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        if (string.IsNullOrEmpty(input.ScriptCode))
            throw Oops.Oh(ErrorCodeEnum.D1701);

        input.CreateFromScript = true;//确保为true
        // 动态创建作业
        var jobType = _dynamicJobCompiler.BuildJob(input.ScriptCode);

        if (jobType.GetCustomAttributes(typeof(JobDetailAttribute)).FirstOrDefault() is not JobDetailAttribute jobDetailAttribute)
            throw Oops.Oh(ErrorCodeEnum.D1702);
        if (jobDetailAttribute.JobId != input.JobId)
            throw Oops.Oh(ErrorCodeEnum.D1703);

        _schedulerFactory.AddJob(
            JobBuilder.Create(jobType).SetIncludeAnnotations(input.IncludeAnnotations)
                .LoadFrom(input.Adapt<SysJobDetail>()).SetJobType(jobType));

        //延迟一下等待持久化写入，再执行其他字段的更新
        await Task.Delay(500);
        await _sysJobDetailRep.AsUpdateable()
            .SetColumns(u => new SysJobDetail { CreateFromScript = input.CreateFromScript, ScriptCode = input.ScriptCode })
            .Where(u => u.JobId == input.JobId).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新作业
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "UpdateJobDetail")]
    [DisplayName("更新作业")]
    public async Task UpdateJobDetail(UpdateJobDetailInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var sysJobDetail = await _sysJobDetailRep.GetFirstAsync(u => u.Id == input.Id);
        if (sysJobDetail.JobId != input.JobId)
            throw Oops.Oh(ErrorCodeEnum.D1704);

        var scheduler = _schedulerFactory.GetJob(sysJobDetail.JobId);
        var oldScriptCode = sysJobDetail.ScriptCode;//旧脚本代码
        input.Adapt(sysJobDetail);

        if (input.CreateFromScript)
        {
            if (string.IsNullOrEmpty(input.ScriptCode))
                throw Oops.Oh(ErrorCodeEnum.D1701);

            if (input.ScriptCode != oldScriptCode)
            {
                // 动态创建作业
                var jobType = _dynamicJobCompiler.BuildJob(input.ScriptCode);

                if (jobType.GetCustomAttributes(typeof(JobDetailAttribute)).FirstOrDefault() is not JobDetailAttribute jobDetailAttribute)
                    throw Oops.Oh(ErrorCodeEnum.D1702);
                if (jobDetailAttribute.JobId != input.JobId)
                    throw Oops.Oh(ErrorCodeEnum.D1703);

                scheduler?.UpdateDetail(JobBuilder.Create(jobType).LoadFrom(sysJobDetail).SetJobType(jobType));
            }
        }
        else
        {
            scheduler?.UpdateDetail(scheduler.GetJobBuilder().LoadFrom(sysJobDetail));
        }

        //tip: 假如这次更新有变更了 JobId，变更 JobId 后触发的持久化更新执行，会由于找不到 JobId 而更新不到数据
        //延迟一下等待持久化写入，再执行其他字段的更新
        await Task.Delay(500);
        await _sysJobDetailRep.UpdateAsync(sysJobDetail);
    }

    /// <summary>
    /// 删除作业
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DeleteJobDetail")]
    [DisplayName("删除作业")]
    public async Task DeleteJobDetail(DeleteJobDetailInput input)
    {
        _schedulerFactory.RemoveJob(input.JobId);

        //如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下面的代码确保作业和触发器能被删除
        await _sysJobDetailRep.DeleteAsync(u => u.JobId == input.JobId);
        await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId);
    }

    /// <summary>
    /// 获取触发器列表
    /// </summary>
    [ApiDescriptionSettings(Name = "JobTriggerList")]
    [DisplayName("获取触发器列表")]
    public async Task<List<SysJobTrigger>> GetJobTriggerList([FromQuery] JobDetailInput input)
    {
        return await _sysJobTriggerRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobId), u => u.JobId.Contains(input.JobId))
            .ToListAsync();
    }

    /// <summary>
    /// 添加触发器
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "AddJobTrigger")]
    [DisplayName("添加触发器")]
    public async Task AddJobTrigger(AddJobTriggerInput input)
    {
        var isExist = await _sysJobTriggerRep.IsAnyAsync(u => u.TriggerId == input.TriggerId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var jobTrigger = input.Adapt<SysJobTrigger>();
        jobTrigger.Args = "[" + jobTrigger.Args + "]";

        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.AddTrigger(Triggers.Create(input.AssemblyName, input.TriggerType).LoadFrom(jobTrigger));
    }

    /// <summary>
    /// 更新触发器
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "UpdateJobTrigger")]
    [DisplayName("更新触发器")]
    public async Task UpdateJobTrigger(UpdateJobTriggerInput input)
    {
        var isExist = await _sysJobTriggerRep.IsAnyAsync(u => u.TriggerId == input.TriggerId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        var jobTrigger = input.Adapt<SysJobTrigger>();
        jobTrigger.Args = "[" + jobTrigger.Args + "]";

        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.UpdateTrigger(Triggers.Create(input.AssemblyName, input.TriggerType).LoadFrom(jobTrigger));
    }

    /// <summary>
    /// 删除触发器
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DeleteJobTrigger")]
    [DisplayName("删除触发器")]
    public async Task DeleteJobTrigger(DeleteJobTriggerInput input)
    {
        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.RemoveTrigger(input.TriggerId);

        //如果 _schedulerFactory 中不存在 JodId，则无法触发持久化，下行代码确保触发器能被删除
        await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId && u.TriggerId == input.TriggerId);
    }

    /// <summary>
    /// 暂停所有作业
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "PauseAllJob")]
    [DisplayName("暂停所有作业")]
    public void PauseAllJob()
    {
        _schedulerFactory.PauseAll();
    }

    /// <summary>
    /// 启动所有作业
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "StartAllJob")]
    [DisplayName("启动所有作业")]
    public void StartAllJob()
    {
        _schedulerFactory.StartAll();
    }

    /// <summary>
    /// 暂停作业
    /// </summary>
    [ApiDescriptionSettings(Name = "PauseJob")]
    [DisplayName("暂停作业")]
    public void PauseJob(JobDetailInput input)
    {
        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.Pause();
    }

    /// <summary>
    /// 启动作业
    /// </summary>
    [ApiDescriptionSettings(Name = "StartJob")]
    [DisplayName("启动作业")]
    public void StartJob(JobDetailInput input)
    {
        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.Start();
    }

    /// <summary>
    /// 暂停触发器
    /// </summary>
    [ApiDescriptionSettings(Name = "PauseTrigger")]
    [DisplayName("暂停触发器")]
    public void PauseTrigger(JobTriggerInput input)
    {
        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.PauseTrigger(input.TriggerId);
    }

    /// <summary>
    /// 启动触发器
    /// </summary>
    [ApiDescriptionSettings(Name = "StartTrigger")]
    [DisplayName("启动触发器")]
    public void StartTrigger(JobTriggerInput input)
    {
        var scheduler = _schedulerFactory.GetJob(input.JobId);
        scheduler?.StartTrigger(input.TriggerId);
    }

    /// <summary>
    /// 强制唤醒作业调度器
    /// </summary>
    [ApiDescriptionSettings(Name = "CancelSleep")]
    [DisplayName("强制唤醒作业调度器")]
    public void CancelSleep()
    {
        _schedulerFactory.CancelSleep();
    }

    /// <summary>
    /// 强制触发所有作业持久化
    /// </summary>
    [ApiDescriptionSettings(Name = "PersistAll")]
    [DisplayName("强制触发所有作业持久化")]
    public void PersistAll()
    {
        _schedulerFactory.PersistAll();
    }

    /// <summary>
    /// 获取集群列表
    /// </summary>
    [ApiDescriptionSettings(Name = "JobClusterList")]
    [DisplayName("获取集群列表")]
    public async Task<List<SysJobCluster>> GetJobClusterList()
    {
        return await _sysJobClusterRep.GetListAsync();
    }
}