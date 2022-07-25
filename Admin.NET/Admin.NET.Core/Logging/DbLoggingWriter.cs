using Furion.Logging;

namespace Admin.NET.Core;

/// <summary>
/// 数据库日志写入器
/// </summary>
public class DbLoggingWriter : IDatabaseLoggingWriter
{
    private readonly SqlSugarRepository<SysLogOp> _sysOpLogRep;

    public DbLoggingWriter(SqlSugarRepository<SysLogOp> sysOpLogRep)
    {
        _sysOpLogRep = sysOpLogRep;
    }

    public async void Write(LogMessage logMsg, bool flush)
    {
        await _sysOpLogRep.InsertAsync(new SysLogOp
        {
            LogName = logMsg.LogName,
            LogLevel = logMsg.LogLevel.ToString(),
            EventId = JSON.Serialize(logMsg.EventId),
            Message = logMsg.Message,
            Exception = logMsg.Exception == null ? "" : JSON.Serialize(logMsg.Exception),
        });
    }
}