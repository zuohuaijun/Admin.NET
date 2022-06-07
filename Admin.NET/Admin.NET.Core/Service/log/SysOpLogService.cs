namespace Admin.NET.Core.Service;

/// <summary>
/// 系统操作日志服务
/// </summary>
[ApiDescriptionSettings(Name = "操作日志", Order = 179)]
public class SysOpLogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysLogOp> _sysOpLogRep;

    public SysOpLogService(SqlSugarRepository<SysLogOp> sysOpLogRep)
    {
        _sysOpLogRep = sysOpLogRep;
    }

    /// <summary>
    /// 获取操作日志分页列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysOpLog/pageList")]
    [NotLog]
    public async Task<SqlSugarPagedList<SysLogOp>> GetOpLogList([FromQuery] PageLogInput input)
    {
        return await _sysOpLogRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                        u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空操作日志
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysOpLog/clear")]
    public async Task<bool> ClearOpLog()
    {
        return await _sysOpLogRep.DeleteAsync(u => u.Id > 0);
    }
}