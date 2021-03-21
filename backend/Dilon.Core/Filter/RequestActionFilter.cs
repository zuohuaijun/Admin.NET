using Furion.DatabaseAccessor.Extensions;
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

            var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var descAtt = Attribute.GetCustomAttribute(actionDescriptor.MethodInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

            var sysOpLog = new SysLogOp
            {
                Name = descAtt != null ? descAtt.Description : actionDescriptor.ActionName,
                OpType = 1,
                Success = isRequestSucceed ? YesOrNot.Y.ToString() : YesOrNot.N.ToString(),
                //Message = isRequestSucceed ? "成功" : "失败",
                Ip = httpContext.GetRemoteIpAddressToIPv4(),
                Location = httpRequest.GetRequestUrlAddress(),
                Browser = clent.UA.Family + clent.UA.Major,
                Os = clent.OS.Family + clent.OS.Major,
                Url = httpRequest.Path,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor.ActionName,
                ReqMethod = httpRequest.Method,
                //Param = JsonSerializerUtility.Serialize(context.ActionArguments),
                //Result = JsonSerializerUtility.Serialize(actionContext.Result),
                ElapsedTime = sw.ElapsedMilliseconds,
                OpTime = DateTimeOffset.Now,
                Account = httpContext.User?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT)
            };
            await sysOpLog.InsertAsync();
        }
    }
}
