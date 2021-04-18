using Furion;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dilon.Core
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 取异常日志服务
            var _logExQueue = App.GetService<IConcurrentQueue<SysLogEx>>();

            // 取用户上下文
            var userContext = App.User;

            // 写入简单队列
            _logExQueue.Add(new SysLogEx
            {
                Account = userContext?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT),
                Name = userContext?.FindFirstValue(ClaimConst.CLAINM_NAME),
                ClassName = context.Exception.TargetSite.DeclaringType.FullName,
                MethodName = context.Exception.TargetSite.Name,
                ExceptionName = context.Exception.Message,
                ExceptionMsg = context.Exception.Message,
                ExceptionSource = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                ParamsObj = context.Exception.TargetSite.GetParameters().ToString(),
                ExceptionTime = DateTimeOffset.Now
            });

            // 写日志
            Log.Error(context.Exception.ToString());

            return Task.CompletedTask;
        }
    }
}
