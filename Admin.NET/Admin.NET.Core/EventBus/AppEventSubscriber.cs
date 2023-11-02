// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 事件订阅
/// </summary>
public class AppEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly IServiceScope _serviceScope;

    public AppEventSubscriber(IServiceScopeFactory scopeFactory)
    {
        _serviceScope = scopeFactory.CreateScope();
    }

    /// <summary>
    /// 增加异常日志
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [EventSubscribe("Add:ExLog")]
    public async Task CreateExLog(EventHandlerExecutingContext context)
    {
        var rep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
        await rep.InsertAsync((SysLogEx)context.Source.Payload);
    }

    /// <summary>
    /// 发送异常邮件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [EventSubscribe("Send:ErrorMail")]
    public async Task SendOrderErrorMail(EventHandlerExecutingContext context)
    {
        //var mailTempPath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Temp\\ErrorMail.tp");
        //var mailTemp = File.ReadAllText(mailTempPath);
        //var mail = await _serviceScope.ServiceProvider.GetRequiredService<IViewEngine>().RunCompileFromCachedAsync(mailTemp, );

        var title = "Admin.NET 框架异常";
        await _serviceScope.ServiceProvider.GetRequiredService<SysMessageService>().SendEmail(JSON.Serialize(context.Source.Payload), title, true);
    }

    /// <summary>
    /// 释放服务作用域
    /// </summary>
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}