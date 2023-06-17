namespace Admin.NET.Core;

/// <summary>
/// Redis 消息扩展
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventConsumer<T> : IDisposable
{
    private Task _consumerTask;
    private CancellationTokenSource _consumerCts;

    /// <summary>
    /// 消费者
    /// </summary>
    public IProducerConsumer<T> Consumer { get; }

    /// <summary>
    /// ConsumerBuilder
    /// </summary>
    public FullRedis Builder { get; set; }

    /// <summary>
    /// 消息回调
    /// </summary>
    public event EventHandler<T> Received;

    /// <summary>
    /// 构造函数
    /// </summary>
    public EventConsumer(FullRedis redis, string routeKey)
    {
        Builder = redis;
        Consumer = Builder.GetQueue<T>(routeKey);
    }

    /// <summary>
    /// 启动
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void Start()
    {
        if (Consumer is null)
        {
            throw new InvalidOperationException("Subscribe first using the Consumer.Subscribe() function");
        }
        if (_consumerTask != null)
        {
            return;
        }
        _consumerCts = new CancellationTokenSource();
        var ct = _consumerCts.Token;
        _consumerTask = Task.Factory.StartNew(() =>
        {
            while (!ct.IsCancellationRequested)
            {
                var cr = Consumer.TakeOne(10);
                if (cr == null) continue;
                Received?.Invoke(this, cr);
            }
        }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <returns></returns>
    public async Task Stop()
    {
        if (_consumerCts == null || _consumerTask == null) return;
        _consumerCts.Cancel();
        try
        {
            await _consumerTask;
        }
        finally
        {
            _consumerTask = null;
            _consumerCts = null;
        }
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_consumerTask != null)
            {
                Stop().Wait();
            }
            Builder.Dispose();
        }
    }
}
