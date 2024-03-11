// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using IPTools.Core;

namespace Admin.NET.Core;

/// <summary>
/// 数据库日志写入器
/// </summary>
public class DatabaseLoggingWriter : IDatabaseLoggingWriter, IDisposable
{
    private readonly IServiceScope _serviceScope;
    private readonly SqlSugarRepository<SysLogVis> _sysLogVisRep; // 访问日志
    private readonly SqlSugarRepository<SysLogOp> _sysLogOpRep; // 操作日志
    private readonly SqlSugarRepository<SysLogEx> _sysLogExRep; // 异常日志
    private readonly SysConfigService _sysConfigService; // 参数配置服务
    private readonly ILogger<DatabaseLoggingWriter> _logger; // 日志组件

    public DatabaseLoggingWriter(IServiceScopeFactory scopeFactory)
    {
        _serviceScope = scopeFactory.CreateScope();
        _sysLogVisRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogVis>>();
        _sysLogOpRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogOp>>();
        _sysLogExRep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
        _sysConfigService = _serviceScope.ServiceProvider.GetRequiredService<SysConfigService>();
        _logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<DatabaseLoggingWriter>>();
    }

    public async void Write(LogMessage logMsg, bool flush)
    {
        var jsonStr = logMsg.Context?.Get("loggingMonitor")?.ToString();
        if (jsonStr == null) return;
        var loggingMonitor = JSON.Deserialize<dynamic>(jsonStr);

        // 不记录数据校验日志
        if (loggingMonitor.validation != null) return;

        // 获取当前操作者
        string account = "", realName = "", userId = "", tenantId = "";
        if (loggingMonitor.authorizationClaims != null)
        {
            foreach (var item in loggingMonitor.authorizationClaims)
            {
                if (item.type == ClaimConst.Account)
                    account = item.value;
                if (item.type == ClaimConst.RealName)
                    realName = item.value;
                if (item.type == ClaimConst.TenantId)
                    tenantId = item.value;
                if (item.type == ClaimConst.UserId)
                    userId = item.value;
            }
        }

        string remoteIPv4 = loggingMonitor.remoteIPv4;
        (string ipLocation, double? longitude, double? latitude) = GetIpAddress(remoteIPv4);

        var client = Parser.GetDefault().Parse(loggingMonitor.userAgent.ToString());
        var browser = $"{client.UA.Family} {client.UA.Major}.{client.UA.Minor} / {client.Device.Family}";
        var os = $"{client.OS.Family} {client.OS.Major} {client.OS.Minor}";

        // 捕捉异常，否则会由于 unhandled exception 导致程序崩溃
        try
        {
            // 记录异常日志并发送邮件
            if (logMsg.Exception != null || loggingMonitor.exception != null)
            {
                await _sysLogExRep.InsertAsync(new SysLogEx
                {
                    ControllerName = loggingMonitor.controllerName,
                    ActionName = loggingMonitor.actionTypeName,
                    DisplayTitle = loggingMonitor.displayTitle,
                    Status = loggingMonitor.returnInformation?.httpStatusCode,
                    RemoteIp = remoteIPv4,
                    Location = ipLocation,
                    Longitude = longitude,
                    Latitude = latitude,
                    Browser = browser, // loggingMonitor.userAgent,
                    Os = os, // loggingMonitor.osDescription + " " + loggingMonitor.osArchitecture,
                    Elapsed = loggingMonitor.timeOperationElapsedMilliseconds,
                    LogDateTime = logMsg.LogDateTime,
                    Account = account,
                    RealName = realName,
                    HttpMethod = loggingMonitor.httpMethod,
                    RequestUrl = loggingMonitor.requestUrl,
                    RequestParam = (loggingMonitor.parameters == null || loggingMonitor.parameters.Count == 0) ? null : JSON.Serialize(loggingMonitor.parameters[0].value),
                    ReturnResult = loggingMonitor.returnInformation == null ? null : JSON.Serialize(loggingMonitor.returnInformation),
                    EventId = logMsg.EventId.Id,
                    ThreadId = logMsg.ThreadId,
                    TraceId = logMsg.TraceId,
                    Exception = JSON.Serialize(loggingMonitor.exception),
                    Message = logMsg.Message,
                    CreateUserId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
                    TenantId = string.IsNullOrWhiteSpace(tenantId) ? 0 : long.Parse(tenantId),
                    LogLevel = logMsg.LogLevel
                });

                // 将异常日志发送到邮件
                await App.GetRequiredService<IEventPublisher>().PublishAsync("Send:ErrorMail", loggingMonitor.exception);

                return;
            }

            // 记录访问日志-登录登出
            if (loggingMonitor.actionName == "userInfo" || loggingMonitor.actionName == "logout")
            {
                await _sysLogVisRep.InsertAsync(new SysLogVis
                {
                    ControllerName = loggingMonitor.controllerName,
                    ActionName = loggingMonitor.actionTypeName,
                    DisplayTitle = loggingMonitor.displayTitle,
                    Status = loggingMonitor.returnInformation?.httpStatusCode,
                    RemoteIp = remoteIPv4,
                    Location = ipLocation,
                    Longitude = longitude,
                    Latitude = latitude,
                    Browser = browser, // loggingMonitor.userAgent,
                    Os = os, // loggingMonitor.osDescription + " " + loggingMonitor.osArchitecture,
                    Elapsed = loggingMonitor.timeOperationElapsedMilliseconds,
                    LogDateTime = logMsg.LogDateTime,
                    Account = account,
                    RealName = realName,
                    CreateUserId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
                    TenantId = string.IsNullOrWhiteSpace(tenantId) ? 0 : long.Parse(tenantId),
                    LogLevel = logMsg.LogLevel
                });

                return;
            }

            // 记录操作日志
            var enabledSysOpLog = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysOpLog);
            if (!enabledSysOpLog) return;
            await _sysLogOpRep.InsertAsync(new SysLogOp
            {
                ControllerName = loggingMonitor.controllerName,
                ActionName = loggingMonitor.actionTypeName,
                DisplayTitle = loggingMonitor.displayTitle,
                Status = loggingMonitor.returnInformation?.httpStatusCode,
                RemoteIp = remoteIPv4,
                Location = ipLocation,
                Longitude = longitude,
                Latitude = latitude,
                Browser = browser, // loggingMonitor.userAgent,
                Os = os, // loggingMonitor.osDescription + " " + loggingMonitor.osArchitecture,
                Elapsed = loggingMonitor.timeOperationElapsedMilliseconds,
                LogDateTime = logMsg.LogDateTime,
                Account = account,
                RealName = realName,
                HttpMethod = loggingMonitor.httpMethod,
                RequestUrl = loggingMonitor.requestUrl,
                RequestParam = (loggingMonitor.parameters == null || loggingMonitor.parameters.Count == 0) ? null : JSON.Serialize(loggingMonitor.parameters[0].value),
                ReturnResult = loggingMonitor.returnInformation == null ? null : JSON.Serialize(loggingMonitor.returnInformation),
                EventId = logMsg.EventId.Id,
                ThreadId = logMsg.ThreadId,
                TraceId = logMsg.TraceId,
                Exception = loggingMonitor.exception == null ? null : JSON.Serialize(loggingMonitor.exception),
                Message = logMsg.Message,
                CreateUserId = string.IsNullOrWhiteSpace(userId) ? 0 : long.Parse(userId),
                TenantId = string.IsNullOrWhiteSpace(tenantId) ? 0 : long.Parse(tenantId),
                LogLevel = logMsg.LogLevel
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    /// <summary>
    /// 解析IP地址
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    internal static (string ipLocation, double? longitude, double? latitude) GetIpAddress(string ip)
    {
        try
        {
            var ipInfo = IpTool.Search(ip);
            var addressList = new List<string>() { ipInfo.Country, ipInfo.Province, ipInfo.City, ipInfo.NetworkOperator };
            return (string.Join("|", addressList.Where(it => it != "0").ToList()), ipInfo.Longitude, ipInfo.Latitude); // 去掉0并用|连接
        }
        catch
        {
            // 不做处理
        }

        return ("未知", 0, 0);
    }

    /// <summary>
    /// 释放服务作用域
    /// </summary>
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}