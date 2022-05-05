using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统异常日志服务
    /// </summary>
    [ApiDescriptionSettings(Name = "系统异常日志", Order = 178)]
    public class SysExLogService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysLogEx> _sysExLogRep;

        public SysExLogService(SqlSugarRepository<SysLogEx> sysExLogRep)
        {
            _sysExLogRep = sysExLogRep;
        }

        /// <summary>
        /// 获取异常日志分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysExLog/pageList")]
        public async Task<SqlSugarPagedList<SysLogEx>> GetExLogList([FromQuery] PageLogInput input)
        {
            return await _sysExLogRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                            u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
                .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc)
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        /// <summary>
        /// 清空异常日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("/sysExLog/clear")]
        public async Task<bool> ClearExLog()
        {
            return await _sysExLogRep.DeleteAsync(u => u.Id > 0);
        }
    }
}