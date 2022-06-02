namespace Admin.NET.Core.Service;

/// <summary>
/// 差异日志服务
/// </summary>
[ApiDescriptionSettings(Name = "差异日志", Order = 180)]
public class SysDiffLogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysLogDiff> _sysDiffLogRep;

    public SysDiffLogService(SqlSugarRepository<SysLogDiff> sysDiffLogRep)
    {
        _sysDiffLogRep = sysDiffLogRep;
    }

    /// <summary>
    /// 获取差异日志分页列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysDiffLog/pageList")]
    [NotLog]
    public async Task<SqlSugarPagedList<SysLogDiff>> GetDiffLogList([FromQuery] PageLogInput input)
    {
        return await _sysDiffLogRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                        u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空差异日志
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysDiffLog/clear")]
    public async Task<bool> ClearVisLog()
    {
        return await _sysDiffLogRep.DeleteAsync(u => u.Id > 0);
    }
}