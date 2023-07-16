// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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