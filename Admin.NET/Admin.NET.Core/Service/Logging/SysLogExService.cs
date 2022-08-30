namespace Admin.NET.Core.Service;

/// <summary>
/// 系统异常日志服务
/// </summary>
[ApiDescriptionSettings(Order = 178)]
public class SysLogExService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysLogEx> _sysLogExRep;

    public SysLogExService(SqlSugarRepository<SysLogEx> sysLogExRep)
    {
        _sysLogExRep = sysLogExRep;
    }

    /// <summary>
    /// 获取异常日志分页列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysLogEx/page")]
    [SuppressMonitor]
    public async Task<SqlSugarPagedList<SysLogEx>> GetLogExPage([FromQuery] PageLogInput input)
    {
        return await _sysLogExRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                        u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空异常日志
    /// </summary>
    /// <returns></returns>
    [HttpPost("/sysLogEx/clear")]
    public async Task<bool> ClearLogEx()
    {
        return await _sysLogExRep.DeleteAsync(u => u.Id > 0);
    }
}