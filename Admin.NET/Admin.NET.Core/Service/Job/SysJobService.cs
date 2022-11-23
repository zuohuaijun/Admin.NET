namespace Admin.NET.Core.Service;

/// <summary>
/// 系统作业任务服务
/// </summary>
[ApiDescriptionSettings(Order = 188)]
[AllowAnonymous]
public class SysJobService : IDynamicApiController, ITransient
{
    private readonly ISchedulerFactory _schedulerFactory;

    public SysJobService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    /// <summary>
    /// 获取所有作业任务列表
    /// </summary>
    [HttpGet("/sysJob/list")]
    public async Task<List<SchedulerModel>> GetJobList()
    {
        var JobDetails = _schedulerFactory.GetJobsOfModels();
        return await Task.FromResult(JobDetails.ToList());
    }

    /// <summary>
    /// 增加作业任务
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/add")]
    public async Task<ScheduleResult> AddJob()
    {
        var jobBuilder = JobBuilder.From(@"
        {
            ""jobId"": ""job1"",
            ""groupName"": null,
            ""jobType"": ""Admin.NET.Application"",
            ""assemblyName"": ""Admin.NET.Application"",
            ""description"": null,
            ""concurrent"": true,
            ""includeAnnotations"": false,
            ""properties"": ""{}"",
            ""updatedTime"": ""2022-11-22 18:00:00""
        }");
        var schedulerResult = _schedulerFactory.TryAddJob(jobBuilder, new[] { Triggers.PeriodSeconds(10) }, out _);
        return await Task.FromResult(schedulerResult);
    }

    /// <summary>
    /// 删除作业任务
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysJob/delete")]
    public async Task<ScheduleResult> DeleteJob(JobInput input)
    {
        var schedulerResult = _schedulerFactory.TryRemoveJob(input.Name, out _);
        return await Task.FromResult(schedulerResult);
    }
}