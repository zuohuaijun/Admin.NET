using IPTools.Core;

namespace Admin.NET.Core;

/// <summary>
/// 数据库日志写入器
/// </summary>
public class DatabaseLoggingWriter : IDatabaseLoggingWriter
{
    private readonly SqlSugarRepository<SysLogVis> _sysLogVisRep; // 访问日志
    private readonly SqlSugarRepository<SysLogOp> _sysLogOpRep;   // 操作日志

    public DatabaseLoggingWriter(SqlSugarRepository<SysLogVis> sysLogVisRep,
        SqlSugarRepository<SysLogOp> sysLogOpRep)
    {
        _sysLogVisRep = sysLogVisRep;
        _sysLogOpRep = sysLogOpRep;
    }

    public async void Write(LogMessage logMsg, bool flush)
    {
        var jsonStr = logMsg.Context.Get("loggingMonitor").ToString();
        dynamic loggingMonitor = JsonConvert.DeserializeObject(jsonStr);

        // 不记录数据校验日志
        if (loggingMonitor.Validation != null) return;

        // 获取当前操作者
        string account = "", realName = "";
        if (loggingMonitor.authorizationClaims != null)
        {
            foreach (var item in loggingMonitor.authorizationClaims)
            {
                if (item.type == ClaimConst.Account)
                    account = item.value;
                if (item.type == ClaimConst.RealName)
                    realName = item.value;
            }
        }

        string remoteIPv4 = loggingMonitor.remoteIPv4;
        (string ipLocation, double? longitude, double? latitude) = GetIpAddress(remoteIPv4);

        if (loggingMonitor.actionName == "login" || loggingMonitor.actionName == "logout")
        {
            if (loggingMonitor.authorizationClaims == null)
            {
                account = logMsg.Context?.Get(ClaimConst.Account)?.ToString();
                realName = logMsg.Context?.Get(ClaimConst.RealName)?.ToString();
            }

            _sysLogVisRep.Insert(new SysLogVis
            {
                ControllerName = loggingMonitor.controllerName,
                ActionName = loggingMonitor.actionTypeName,
                DisplayTitle = loggingMonitor.displayTitle,
                Status = loggingMonitor.returnInformation.httpStatusCode,
                RemoteIp = remoteIPv4,
                Location = ipLocation,
                Longitude = longitude,
                Latitude = latitude,
                Browser = loggingMonitor.userAgent,
                Os = loggingMonitor.osDescription + " " + loggingMonitor.osArchitecture,
                Elapsed = loggingMonitor.timeOperationElapsedMilliseconds,
                LogDateTime = logMsg.LogDateTime,
                Account = account,
                RealName = realName
            });
        }
        else
        {
            _sysLogOpRep.Insert(new SysLogOp
            {
                ControllerName = loggingMonitor.controllerName,
                ActionName = loggingMonitor.actionTypeName,
                DisplayTitle = loggingMonitor.displayTitle,
                Status = loggingMonitor.returnInformation.httpStatusCode,
                RemoteIp = remoteIPv4,
                Location = ipLocation,
                Longitude = longitude,
                Latitude = latitude,
                Browser = loggingMonitor.userAgent,
                Os = loggingMonitor.osDescription + " " + loggingMonitor.osArchitecture,
                Elapsed = loggingMonitor.timeOperationElapsedMilliseconds,
                LogDateTime = logMsg.LogDateTime,
                Account = account,
                RealName = realName,
                HttpMethod = loggingMonitor.httpMethod,
                RequestUrl = loggingMonitor.requestUrl,
                RequestParam = (loggingMonitor.parameters == null || loggingMonitor.parameters.Count == 0) ? null : JsonConvert.SerializeObject(loggingMonitor.parameters[0].value),
                ReturnResult = JsonConvert.SerializeObject(loggingMonitor.returnInformation.value),
                EventId = logMsg.EventId.Id,
                ThreadId = logMsg.ThreadId,
                TraceId = logMsg.TraceId,
                Exception = loggingMonitor.exception,
                Message = logMsg.Message
            });
        }

        // 异常时发送邮件
        if (logMsg.Exception != null)
            await App.GetRequiredService<SysMessageService>().SendEmail(loggingMonitor.exception);
    }

    /// <summary>
    /// 解析IP地址
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    private static (string ipLocation, double? longitude, double? latitude) GetIpAddress(string ip)
    {
        try
        {
            var ipInfo = IpTool.Search(ip);
            var addressList = new List<string>() { ipInfo.Country, ipInfo.Province, ipInfo.City, ipInfo.NetworkOperator };
            return (string.Join("|", addressList.Where(it => it != "0").ToList()), ipInfo.Longitude, ipInfo.Latitude); // 去掉0并用|连接
        }
        catch { }
        return ("未知", 0, 0);
    }
}