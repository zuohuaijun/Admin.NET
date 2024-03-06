// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Elasticsearch.Net;
using Nest;

namespace Admin.NET.Core;

/// <summary>
/// ES服务注册
/// </summary>
public static class ElasticSearchSetup
{
    public static void AddElasticSearch(this IServiceCollection services)
    {
        var enabled = App.GetConfig<bool>("Logging:ElasticSearch:Enabled", true);
        if (!enabled) return;

        var serverUris = App.GetConfig<List<string>>("Logging:ElasticSearch:ServerUris", true);
        var defaultIndex = App.GetConfig<string>("Logging:ElasticSearch:DefaultIndex", true);

        var uris = serverUris.Select(u => new Uri(u));
        // 集群
        var connectionPool = new SniffingConnectionPool(uris);
        var connectionSettings = new ConnectionSettings(connectionPool).DefaultIndex(defaultIndex);
        // 单连接
        //var connectionSettings = new ConnectionSettings(new SingleNodeConnectionPool(uris.FirstOrDefault())).DefaultIndex(defaultIndex);
        var client = new ElasticClient(connectionSettings);
        client.Indices.Create(defaultIndex, u => u.Map<SysLogOp>(m => m.AutoMap()));

        services.AddSingleton(client); // 单例注册
    }
}