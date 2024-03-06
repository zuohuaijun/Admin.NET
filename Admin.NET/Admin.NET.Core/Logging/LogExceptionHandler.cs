// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

//using Microsoft.AspNetCore.Mvc.Controllers;
//using System.Security.Claims;

//namespace Admin.NET.Core.Logging;

///// <summary>
///// 全局异常处理
///// </summary>
//public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
//{
//    private readonly IEventPublisher _eventPublisher;

//    public LogExceptionHandler(IEventPublisher eventPublisher)
//    {
//        _eventPublisher = eventPublisher;
//    }

//    public async Task OnExceptionAsync(ExceptionContext context)
//    {
//        var actionMethod = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
//        var displayNameAttribute = actionMethod.IsDefined(typeof(DisplayNameAttribute), true) ? actionMethod.GetCustomAttribute<DisplayNameAttribute>(true) : default;

//        var sysLogEx = new SysLogEx
//        {
//            Account = App.User?.FindFirstValue(ClaimConst.Account),
//            RealName = App.User?.FindFirstValue(ClaimConst.RealName),
//            ControllerName = actionMethod.DeclaringType.FullName,
//            ActionName = actionMethod.Name,
//            DisplayTitle = displayNameAttribute?.DisplayName,
//            Exception = $"异常信息:{context.Exception.Message} 异常来源：{context.Exception.Source} 堆栈信息：{context.Exception.StackTrace}",
//            Message = "全局异常",
//            RequestParam = context.Exception.TargetSite.GetParameters().ToString(),
//            HttpMethod = context.HttpContext.Request.Method,
//            RequestUrl = context.HttpContext.Request.GetRequestUrlAddress(),
//            RemoteIp = context.HttpContext.GetRemoteIpAddressToIPv4(),
//            Browser = context.HttpContext.Request.Headers["User-Agent"],
//            TraceId = App.GetTraceId(),
//            ThreadId = App.GetThreadId(),
//            LogDateTime = DateTime.Now,
//            LogLevel = LogLevel.Error
//        };

//        await _eventPublisher.PublishAsync(new ChannelEventSource("Add:ExLog", sysLogEx));

//        await _eventPublisher.PublishAsync("Send:ErrorMail", sysLogEx);
//    }
//}