// Apache-2.0 License
// Copyright (c) 2021-2022 xusn

using Nest;

namespace Admin.NET.Core;
/// <summary>
/// ES日志类
/// </summary>
public class ElasticSearchLogMessage
{

    /// <summary>
    /// 记录器类别名称
    /// </summary>
    [Text] public string LogName { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    [Text] public string LogLevel { get; set; }

    /// <summary>
    /// 事件 Id
    /// </summary>
    [Text] public string EventId { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    [Text] public string Message { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    [Text] public string Exception { get; set; }

    /// <summary>
    /// 当前状态值
    /// </summary>
    /// <remarks>可以是任意类型</remarks>
    [Text] public string State { get; set; }

    /// <summary>
    /// 日志记录时间
    /// </summary>
    [Text] public DateTime LogDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 线程 Id
    /// </summary>
    [Text] public int ThreadId { get; set; }

    /// <summary>
    /// 是否使用 UTC 时间戳
    /// </summary>
    [Text] public bool UseUtcTimestamp { get; set; }

    /// <summary>
    /// 请求/跟踪 Id
    /// </summary>
    [Text] public string TraceId { get; set; }

    /// <summary>
    /// 监视
    /// </summary>
    [Text] public string Monitor { get; set; }

}
