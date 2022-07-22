using Serilog;

namespace Admin.NET.Core;

/// <summary>
/// 全局异常处理器
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
        var stackTrace = EnhancedStackTrace.Current();
        await _eventPublisher.PublishAsync("Add:ExLog", new SysLogEx
        {
            ClassName = context.Exception.TargetSite.DeclaringType?.FullName,
            MethodName = context.Exception.TargetSite.Name,
            ExceptionName = context.Exception.Message,
            ExceptionMsg = context.Exception.Message,
            ExceptionSource = context.Exception.Source,
            StackTrace = stackTrace.ToString(), // context.Exception.StackTrace,
            ParamsObj = context.Exception.TargetSite.GetParameters().ToString(),
            UserName = context.HttpContext.User?.FindFirst(ClaimConst.UserName)?.Value,
            RealName = context.HttpContext.User?.FindFirst(ClaimConst.RealName)?.Value
        });

        // 写日志文件
        Log.Error(stackTrace.ToString());
    }
}