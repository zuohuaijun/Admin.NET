using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 异常日志服务
    /// </summary>
    [ApiDescriptionSettings(Name = "ExLog", Order = 100)]
    public class SysExLogService : ISysExLogService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysLogEx> _sysExLogRep;  // 操作日志表仓储

        public SysExLogService(IRepository<SysLogEx> sysExLogRep)
        {
            _sysExLogRep = sysExLogRep;
        }

        /// <summary>
        /// 分页查询异常日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysExLog/page")]
        public async Task<dynamic> QueryExLogPageList([FromQuery] ExLogPageInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var className = !string.IsNullOrEmpty(input.ClassName?.Trim());
            var methodName = !string.IsNullOrEmpty(input.MethodName?.Trim());
            var exceptionMsg = !string.IsNullOrEmpty(input.ExceptionMsg?.Trim());
            var searchBeginTime = !string.IsNullOrEmpty(input.SearchBeginTime?.Trim());
            var exLogs = await _sysExLogRep.DetachedEntities
                                           .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")))
                                           .Where(className, u => u.ClassName == input.ClassName)
                                           .Where(methodName, u => u.MethodName == input.MethodName)
                                           .Where(exceptionMsg, u => EF.Functions.Like(u.ExceptionMsg, $"%{input.ExceptionMsg.Trim()}%"))
                                           .Where(searchBeginTime, u => u.ExceptionTime >= DateTime.Parse(input.SearchBeginTime.Trim()) &&
                                                                   u.ExceptionTime <= DateTime.Parse(input.SearchEndTime.Trim()))
                                           .OrderByDescending(u => u.Id)
                                           .Select(u => u.Adapt<ExLogOutput>())
                                           .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<ExLogOutput>.PageResult(exLogs);
        }

        /// <summary>
        /// 清空异常日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("/sysExLog/delete")]
        public async Task ClearExLog()
        {
            var exLogs = await _sysExLogRep.Entities.ToListAsync();
            await _sysExLogRep.DeleteAsync(exLogs);
        }
    }
}