using Furion.JsonSerialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Furion.TaskScheduler;
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
            var headers = httpRequest.Headers;
            var clientInfo = headers.ContainsKey("User-Agent")
                ? Parser.GetDefault().Parse(headers["User-Agent"])
                : null;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            // _ = Attribute.GetCustomAttribute(actionDescriptor.MethodInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
            
            // 日志写入简单队列
            var sysLogOp = new SysLogOp
            {
                Name = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_NAME), //0
                Success = isRequestSucceed ? YesOrNot.Y : YesOrNot.N,
                Ip = httpContext.GetRemoteIpAddressToIPv4(), //0
                Location = httpRequest.GetRequestUrlAddress(), //0
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                Os = clientInfo?.OS.Family + clientInfo?.OS.Major,
                Url = httpRequest.Path, //0
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor?.ActionName,
                ReqMethod = httpRequest.Method, //0
                Param = JSON.Serialize(context.ActionArguments.Count < 1 ? "" : context.ActionArguments), //0
                //Result = JSON.Serialize(actionContext.Result), // 序列化异常，比如验证码
                ElapsedTime = sw.ElapsedMilliseconds,
                OpTime = DateTimeOffset.Now,
                Account = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT) //0
            };
            SpareTime.DoIt(() =>
            {
                SimpleQueue<SysLogOp>.Add(sysLogOp);
            });
        }
    }
}
