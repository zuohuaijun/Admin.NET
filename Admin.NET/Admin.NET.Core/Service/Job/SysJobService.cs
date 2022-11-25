namespace Admin.NET.Core.Service;

/// <summary>
/// 系统作业任务服务
/// </summary>
[ApiDescriptionSettings(Order = 188)]
public class SysJobService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysJobDetail> _sysJobDetailRep;
    private readonly SqlSugarRepository<SysJobTrigger> _sysJobTriggerRep;
    private readonly SqlSugarRepository<SysJobCluster> _sysJobClusterRep;
    private readonly ISchedulerFactory _schedulerFactory;

    public SysJobService(SqlSugarRepository<SysJobDetail> sysJobDetailRep,
        SqlSugarRepository<SysJobTrigger> sysJobTriggerRep,
        SqlSugarRepository<SysJobCluster> sysJobClusterRep,
        ISchedulerFactory schedulerFactory)
    {
        _sysJobDetailRep = sysJobDetailRep;
        _sysJobTriggerRep = sysJobTriggerRep;
        _sysJobClusterRep = sysJobClusterRep;
        _schedulerFactory = schedulerFactory;
    }

    /// <summary>
    /// 获取作业分页列表
    /// </summary>
    [HttpGet("/sysJob/page")]
    public async Task<SqlSugarPagedList<JobOutput>> GetJobPage([FromQuery] PageJobInput input)
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
        return jobDetails;
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/detailAdd")]
    public async Task AddJobDetail(AddJobDetailInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        await _sysJobDetailRep.UpdateAsync(input.Adapt<SysJobDetail>());
    }

    /// <summary>
    /// 更新作业
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/detailUpdate")]
    public async Task UpdateJobDetail(UpdateJobDetailInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        await _sysJobDetailRep.UpdateAsync(input.Adapt<SysJobDetail>());
    }

    /// <summary>
    /// 删除作业
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/detailDelete")]
    public async Task<ScheduleResult> DeleteJobDetail(DeleteJobDetailInput input)
    {
        await _sysJobDetailRep.DeleteAsync(u => u.JobId == input.JobId);
        await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId);

        var schedulerResult = _schedulerFactory.TryRemoveJob(input.JobId, out _);
        return await Task.FromResult(schedulerResult);
    }

    /// <summary>
    /// 获取触发器列表
    /// </summary>
    [HttpGet("/sysJob/triggerList")]
    public async Task<List<SysJobTrigger>> GetJobTriggerList([FromQuery] JobDetailInput input)
    {
        return await _sysJobTriggerRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.JobId), u => u.JobId.Contains(input.JobId))
            .ToListAsync();
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/triggerAdd")]
    public async Task AddJobTrigger(AddJobTriggerInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        await _sysJobDetailRep.UpdateAsync(input.Adapt<SysJobDetail>());
    }

    /// <summary>
    /// 更新触发器
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/triggerUpdate")]
    public async Task UpdateJobTrigger(UpdateJobTriggerInput input)
    {
        var isExist = await _sysJobDetailRep.IsAnyAsync(u => u.JobId == input.JobId && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1006);

        await _sysJobDetailRep.UpdateAsync(input.Adapt<SysJobDetail>());
    }

    /// <summary>
    /// 删除触发器
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/triggerDelete")]
    public async Task DeleteJobTrigger(DeleteJobTriggerInput input)
    {
        await _sysJobTriggerRep.DeleteAsync(u => u.JobId == input.JobId);
    }

    /// <summary>
    /// 获取集群列表
    /// </summary>
    [HttpGet("/sysJob/clusterList")]
    public async Task<List<SysJobCluster>> GetJobClusterList()
    {
        return await _sysJobClusterRep.GetListAsync();
    }
}