//// 麻省理工学院许可证
////
//// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
////
//// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
////
//// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
//// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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