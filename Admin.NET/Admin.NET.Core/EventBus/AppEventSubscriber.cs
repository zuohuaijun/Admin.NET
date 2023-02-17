namespace Admin.NET.Core;

/// <summary>
/// 事件订阅
/// </summary>
public class AppEventSubscriber : IEventSubscriber, ISingleton
{
    private readonly IServiceProvider _serviceProvider;

    public AppEventSubscriber(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    ///// <summary>
    ///// 增加异常日志
    ///// </summary>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //[EventSubscribe("Add:ExLog")]
    //public async Task CreateExLog(EventHandlerExecutingContext context)
    //{
    //    using var scope = _serviceProvider.CreateScope();
    //    var _rep = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
    //    await _rep.InsertAsync((SysLogEx)context.Source.Payload);

    //    // 发送邮件
    //    await scope.ServiceProvider.GetRequiredService<SysMessageService>().SendEmail(JSON.Serialize(context.Source.Payload));
    //}
}