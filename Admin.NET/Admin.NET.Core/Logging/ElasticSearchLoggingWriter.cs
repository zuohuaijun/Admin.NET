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
        _esClient.IndexDocument(JSON.Serialize(logMsg));
    }
}