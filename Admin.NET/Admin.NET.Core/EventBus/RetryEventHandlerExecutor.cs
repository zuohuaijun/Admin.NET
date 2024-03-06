// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 事件执行器-超时控制、失败重试熔断等等
/// </summary>
public class RetryEventHandlerExecutor : IEventHandlerExecutor
{
    public async Task ExecuteAsync(EventHandlerExecutingContext context, Func<EventHandlerExecutingContext, Task> handler)
    {
        // 如果执行失败，每隔 1s 重试，最多三次
        await Retry.InvokeAsync(async () =>
        {
            await handler(context);
        }, 3, 1000);
    }
}