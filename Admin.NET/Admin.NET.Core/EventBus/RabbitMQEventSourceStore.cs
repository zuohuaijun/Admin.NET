// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace Admin.NET.Core;

/// <summary>
/// RabbitMQ自定义事件源存储器
/// </summary>
public class RabbitMQEventSourceStore : IEventSourceStorer
{
    /// <summary>
    /// 内存通道事件源存储器
    /// </summary>
    private readonly Channel<IEventSource> _channel;

    /// <summary>
    /// 通道对象
    /// </summary>
    private readonly IModel _model;

    /// <summary>
    /// 连接对象
    /// </summary>
    private readonly IConnection _connection;

    /// <summary>
    /// 路由键
    /// </summary>
    private readonly string _routeKey;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="factory">连接工厂</param>
    /// <param name="routeKey">路由键</param>
    /// <param name="capacity">存储器最多能够处理多少消息，超过该容量进入等待写入</param>
    public RabbitMQEventSourceStore(ConnectionFactory factory, string routeKey, int capacity)
    {
        // 配置通道，设置超出默认容量后进入等待
        var boundedChannelOptions = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        // 创建有限容量通道
        _channel = Channel.CreateBounded<IEventSource>(boundedChannelOptions);

        // 创建连接
        _connection = factory.CreateConnection();
        _routeKey = routeKey;

        // 创建通道
        _model = _connection.CreateModel();

        // 声明路由队列
        _model.QueueDeclare(routeKey, false, false, false, null);

        // 创建消息订阅者
        var consumer = new EventingBasicConsumer(_model);

        // 订阅消息并写入内存 Channel
        consumer.Received += (ch, ea) =>
        {
            // 读取原始消息
            var stringEventSource = Encoding.UTF8.GetString(ea.Body.ToArray());

            // 转换为 IEventSource，如果自定义了 EventSource，注意属性是可读可写
            var eventSource = JSON.Deserialize<ChannelEventSource>(stringEventSource);

            // 写入内存管道存储器
            _channel.Writer.WriteAsync(eventSource);

            // 确认该消息已被消费
            _model.BasicAck(ea.DeliveryTag, false);
        };

        // 启动消费者且设置为手动应答消息
        _model.BasicConsume(routeKey, false, consumer);
    }

    /// <summary>
    /// 将事件源写入存储器
    /// </summary>
    /// <param name="eventSource">事件源对象</param>
    /// <param name="cancellationToken">取消任务 Token</param>
    /// <returns><see cref="ValueTask"/></returns>
    public async ValueTask WriteAsync(IEventSource eventSource, CancellationToken cancellationToken)
    {
        if (eventSource == default)
            throw new ArgumentNullException(nameof(eventSource));

        // 判断是否是 ChannelEventSource 或自定义的 EventSource
        if (eventSource is ChannelEventSource source)
        {
            // 序列化及发布
            var data = Encoding.UTF8.GetBytes(JSON.Serialize(source));
            _model.BasicPublish("", _routeKey, null, data);
        }
        else
        {
            // 处理动态订阅
            await _channel.Writer.WriteAsync(eventSource, cancellationToken);
        }
    }

    /// <summary>
    /// 从存储器中读取一条事件源
    /// </summary>
    /// <param name="cancellationToken">取消任务 Token</param>
    /// <returns>事件源对象</returns>
    public async ValueTask<IEventSource> ReadAsync(CancellationToken cancellationToken)
    {
        var eventSource = await _channel.Reader.ReadAsync(cancellationToken);
        return eventSource;
    }

    /// <summary>
    /// 释放非托管资源
    /// </summary>
    public void Dispose()
    {
        _model.Dispose();
        _connection.Dispose();
    }
}