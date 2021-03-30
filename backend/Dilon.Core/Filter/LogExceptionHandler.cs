using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
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
            //context.Exception.ToString().LogError("错误");

            // 写日志
            Log.Error(context.Exception.ToString());

            return Task.CompletedTask;
        }
    }
}
