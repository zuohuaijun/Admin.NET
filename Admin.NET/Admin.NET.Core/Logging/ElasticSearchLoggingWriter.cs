using Microsoft.Extensions.Logging;
using Nest;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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
        _esClient.IndexDocument(new LogContent
        {
            Time = DateTime.Now,
            LogLevel = logMsg.LogLevel,
            LogName = logMsg.LogName,
            EventId = logMsg.EventId,
            Message = logMsg.Message,
            Exception = logMsg.Exception
        });
    }
}

/// <summary>
/// 日志内容
/// </summary>
public class LogContent
{
    /// <summary>
    /// 记录器类别名称
    /// </summary>
    public string LogName { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    ///  事件 Id
    /// </summary>
    public EventId EventId { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// 日志上下文
    /// </summary>
    public LogContext Context { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    public string Message { get; internal set; }

    /// <summary>
    /// 日志时间
    /// </summary>
    public DateTime Time { get; internal set; }
}