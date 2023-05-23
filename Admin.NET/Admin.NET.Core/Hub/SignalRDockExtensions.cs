using StackExchange.Redis;

namespace Admin.NET.Core;

public static class SignalRDockExtensions
{
    /// <summary>
    /// 注入redis底板
    /// </summary>
    /// <param name="services"></param>
    /// <param name="delegateRedisConnectionString"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddSignalR_RedisDock(this IServiceCollection services, Func<string> delegateRedisConnectionString)
    {
        var redisConnectionString = delegateRedisConnectionString();
        if (string.IsNullOrEmpty(redisConnectionString))
            throw new ArgumentNullException(nameof(redisConnectionString));

        services.AddSignalR(hubOptions =>
            {
                // SignalR 自己的 pinger ,客户端在定义的时间跨度内没有响应，它将触发OnDisconnectedAsync
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(5);
            })
            .AddJsonProtocol()
            .AddMessagePackProtocol()
            // 支持MessagePack
            .AddStackExchangeRedis(options =>
            {
                options.ConnectionFactory = async writer =>
                {

                    var config = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        // Password = "changeme",
                        ChannelPrefix = "__signalr_",
                    };
                    // config.EndPoints.Add(IPAddress.Loopback, 0);
                    // config.SetDefaultPorts();
                    config.DefaultDatabase = 1;
                    var connection = await ConnectionMultiplexer.ConnectAsync(redisConnectionString, writer);
                    connection.ConnectionFailed += (_, e) =>
                    {
                        Console.WriteLine("Connection to Redis failed.");
                    };

                    Console.WriteLine(connection.IsConnected ? "connected to Redis." : "Did not connect to Redis");

                    return connection;
                };
            });
    }
}


//redis查看订阅列表 PUBSUB CHANNELS
