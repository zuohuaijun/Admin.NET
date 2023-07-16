// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Nest;

namespace Admin.NET.Core;

/// <summary>
/// ES日志写入器
/// </summary>
public class ElasticSearchLoggingWriter : IDatabaseLoggingWriter
{
    private readonly ElasticClient _esClient;
    private readonly SysConfigService _sysConfigService; // 参数配置服务

    public ElasticSearchLoggingWriter(ElasticClient esClient, SysConfigService sysConfigService)
    {
        _esClient = esClient;
        _sysConfigService = sysConfigService;
    }

    public async void Write(LogMessage logMsg, bool flush)
    {
        // 是否启用操作日志
        var sysOpLogEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysOpLog);
        if (!sysOpLogEnabled) return;

        var jsonStr = logMsg.Context.Get("loggingMonitor").ToString();
        var loggingMonitor = JSON.Deserialize<dynamic>(jsonStr);

        // 不记录登录登出日志
        if (loggingMonitor.actionName == "userInfo" || loggingMonitor.actionName == "logout")
            return;

        await _esClient.IndexDocumentAsync(jsonStr);
    }
}