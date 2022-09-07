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
        var elkOptions = App.GetOptions<ElasticSearchOptions>();

        var uris = elkOptions.ServerUris.Select(u => new Uri(u));
        var connectionPool = new SniffingConnectionPool(uris);
        var settings = new ConnectionSettings(connectionPool).DefaultIndex(elkOptions.DefaultIndex);
        var client = new ElasticClient(settings);

        services.AddSingleton(client); // 单例注册
    }
}