namespace Admin.NET.Core.Service;

/// <summary>
/// 系统访问日志服务
/// </summary>
[ApiDescriptionSettings(Order = 350)]
public class SysLogVisService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysLogVis> _sysLogVisRep;

    public SysLogVisService(SqlSugarRepository<SysLogVis> sysLogVisRep)
    {
        _sysLogVisRep = sysLogVisRep;
    }

    /// <summary>
    /// 获取访问日志分页列表
    /// </summary>
    /// <returns></returns>
    [SuppressMonitor]
    [DisplayName("获取访问日志分页列表")]
    public async Task<SqlSugarPagedList<SysLogVis>> GetPage([FromQuery] PageLogInput input)
    {
        return await _sysLogVisRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 清空访问日志
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Clear"), HttpPost]
    [DisplayName("清空访问日志")]
    public async Task<bool> Clear()
    {
        return await _sysLogVisRep.DeleteAsync(u => u.Id > 0);
    }
}