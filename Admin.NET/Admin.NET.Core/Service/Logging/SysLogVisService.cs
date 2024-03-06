// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统访问日志服务
/// </summary>
[ApiDescriptionSettings(Order = 340)]
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
    public async Task<SqlSugarPagedList<SysLogVis>> Page(PageLogInput input)
    {
        return await _sysLogVisRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()), u => u.CreateTime >= input.StartTime)
            .WhereIF(!string.IsNullOrWhiteSpace(input.EndTime.ToString()), u => u.CreateTime <= input.EndTime)
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