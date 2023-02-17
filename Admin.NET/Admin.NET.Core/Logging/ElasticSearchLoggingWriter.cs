using Nest;

namespace Admin.NET.Core;

/// <summary>
/// ES日志写入器
/// </summary>
public class ElasticSearchLoggingWriter : IDatabaseLoggingWriter
{
    private readonly ElasticClient _esClient;

    public ElasticSearchLoggingWriter(ElasticClient esClient)
    {
        _esClient = esClient;
    }

    public void Write(LogMessage logMsg, bool flush)
    {
        var jsonStr = logMsg.Context.Get("loggingMonitor").ToString();
        // dynamic loggingMonitor = JsonConvert.DeserializeObject(jsonStr);
        _esClient.IndexDocument(jsonStr);
    }
}