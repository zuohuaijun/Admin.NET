using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统访问日志服务
    /// </summary>
    [ApiDescriptionSettings(Name = "系统访问日志", Order = 180)]
    public class SysVisLogService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysLogVis> _sysVisLogRep;

        public SysVisLogService(SqlSugarRepository<SysLogVis> sysVisLogRep)
        {
            _sysVisLogRep = sysVisLogRep;
        }

        /// <summary>
        /// 获取访问日志分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysVisLog/pageList")]
        public async Task<List<SysLogVis>> GetVisLogList([FromQuery] LogInput input)
        {
            return await _sysVisLogRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                            u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
                .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 清空访问日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("/sysVisLog/clear")]
        public async Task<bool> ClearVisLog()
        {
            return await _sysVisLogRep.DeleteAsync(u => u.Id > 0);
        }
    }
}