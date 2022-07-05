namespace Admin.NET.Core;

/// <summary>
/// SqlSugar工作单元模式
/// </summary>
[SuppressSniffer]
public sealed class SqlSugarUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// SqlSugar 对象
    /// </summary>
    private readonly SqlSugarClient _sqlSugarClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sqlSugarClient"></param>
    public SqlSugarUnitOfWork(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = (SqlSugarClient)sqlSugarClient;
    }

    /// <summary>
    /// 工作单元未标记处理
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="isManual"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnUnmark(ActionExecutedContext resultContext, bool isManual)
    {
    }

    /// <summary>
    /// 开启工作单元处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="unitOfwork"></param>
    /// <param name="isManual"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void BeginTransaction(ActionExecutingContext context, UnitOfWorkAttribute unitOfwork, bool isManual)
    {
        _sqlSugarClient.BeginTran();
    }

    /// <summary>
    /// 提交工作单元处理
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="unitOfwork"></param>
    /// <param name="isManual"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void CommitTransaction(ActionExecutedContext resultContext, UnitOfWorkAttribute unitOfwork, bool isManual)
    {
        _sqlSugarClient.CommitTran();
    }

    /// <summary>
    /// 回滚工作单元处理
    /// </summary>
    /// <param name="resultContext"></param>
    /// <param name="unitOfwork"></param>
    /// <param name="isManual"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void RollbackTransaction(ActionExecutedContext resultContext, UnitOfWorkAttribute unitOfwork, bool isManual)
    {
        _sqlSugarClient.RollbackTran();
    }

    /// <summary>
    /// 执行完毕（无论成功失败）
    /// </summary>
    /// <param name="context"></param>
    /// <param name="resultContext"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnCompleted(ActionExecutingContext context, ActionExecutedContext resultContext)
    {
        _sqlSugarClient.Dispose();
    }
}