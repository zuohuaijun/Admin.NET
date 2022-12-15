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
        var document = new ElasticSearchLogMessage
        {
            EventId = logMsg.EventId.Id.ToString(),
            LogName = logMsg.LogName,
            LogLevel = logMsg.LogLevel.ToString(),
            Message = logMsg.Message,
            Exception = logMsg.Exception?.ToString(),
            State = logMsg.State.ToString(),
            LogDateTime = logMsg.LogDateTime,
            ThreadId = logMsg.ThreadId,
            UseUtcTimestamp = logMsg.UseUtcTimestamp,
            TraceId = logMsg.TraceId,
            Monitor = logMsg.Context?.Get("loggingMonitor")?.ToString(),
        };
        _esClient.IndexDocument(document);
    }
}