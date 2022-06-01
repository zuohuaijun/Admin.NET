namespace Admin.NET.Core;

/// <summary>
/// 全局异常处理
/// </summary>
public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
{
    private readonly IEventPublisher _eventPublisher;

    public LogExceptionHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        await _eventPublisher.PublishAsync(new ChannelEventSource("Add:ExLog",
            new SysLogEx
            {
                ClassName = context.Exception.TargetSite.DeclaringType?.FullName,
                MethodName = context.Exception.TargetSite.Name,
                ExceptionName = context.Exception.Message,
                ExceptionMsg = context.Exception.Message,
                ExceptionSource = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                ParamsObj = context.Exception.TargetSite.GetParameters().ToString(),
                UserName = context.HttpContext.User?.FindFirst(ClaimConst.UserName)?.Value,
                RealName = context.HttpContext.User?.FindFirst(ClaimConst.RealName)?.Value
            }));

        // 写日志文件
        Log.Error(context.Exception.ToString());
    }
}