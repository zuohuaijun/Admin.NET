// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace Admin.Core;

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

            // 转换为 IEventSource，这里可以选择自己喜欢的序列化工具，如果自定义了 EventSource，注意属性是可读可写
            var eventSource = JSON.Deserialize<ChannelEventSource>(stringEventSource);

            // 写入内存管道存储器
            _channel.Writer.WriteAsync(eventSource);

            // 确认该消息已被消费
            _model.BasicAck(ea.DeliveryTag, false);
        };

        // 启动消费者 设置为手动应答消息
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
        // 空检查
        if (eventSource == default)
        {
            throw new ArgumentNullException(nameof(eventSource));
        }

        // 这里判断是否是 ChannelEventSource 或者 自定义的 EventSource
        if (eventSource is ChannelEventSource source)
        {
            // 序列化，这里可以选择自己喜欢的序列化工具
            var data = Encoding.UTF8.GetBytes(JSON.Serialize(source));

            // 发布
            _model.BasicPublish("", _routeKey, null, data);
        }
        else
        {
            // 这里处理动态订阅问题
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
        // 读取一条事件源
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