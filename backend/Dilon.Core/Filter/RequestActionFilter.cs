using Furion;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using UAParser;

namespace Dilon.Core
{
    /// <summary>
    /// 请求日志拦截
    /// </summary>
    public class RequestActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;

            var sw = new Stopwatch();
            sw.Start();

            var actionContext = await next();
            sw.Stop();

            // 判断是否请求成功（没有异常就是请求成功）
            var isRequestSucceed = actionContext.Exception == null;
            var clientInfo = httpContext.Request.Headers.ContainsKey("User-Agent") ?
                Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]) : null;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            // _ = Attribute.GetCustomAttribute(actionDescriptor.MethodInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

            // 日志写入简单队列
            var _logOpQueue = App.GetService<IConcurrentQueue<SysLogOp>>();
            _logOpQueue.Add(new SysLogOp
            {
                Name = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_NAME),
                Success = isRequestSucceed ? YesOrNot.Y : YesOrNot.N,
                //Message = isRequestSucceed ? "成功" : "失败",
                Ip = httpContext.GetRemoteIpAddressToIPv4(),
                Location = httpRequest.GetRequestUrlAddress(),
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
                Url = httpRequest.Path,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor.ActionName,
                ReqMethod = httpRequest.Method,
                Param = JSON.Serialize(context.ActionArguments),
                Result = JSON.Serialize(actionContext.Result),
                ElapsedTime = sw.ElapsedMilliseconds,
                OpTime = DateTimeOffset.Now,
                Account = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT)
            });
        }
    }
}
