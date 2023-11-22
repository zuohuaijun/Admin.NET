// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Furion.Logging.Extensions;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace Admin.NET.Core;

public static class SignalRSetup
{
    /// <summary>
    /// 即时消息SignalR注册
    /// </summary>
    /// <param name="services"></param>
    /// <param name="SetNewtonsoftJsonSetting"></param>
    public static void AddSignalR(this IServiceCollection services, Action<JsonSerializerSettings> SetNewtonsoftJsonSetting)
    {
        var signalRBuilder = services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
            options.KeepAliveInterval = TimeSpan.FromMinutes(1);
            options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 数据包大小10M，默认最大为32K
        }).AddNewtonsoftJsonProtocol(options => SetNewtonsoftJsonSetting(options.PayloadSerializerSettings));

        // 若未启用Redis缓存，直接返回
        var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        if (cacheOptions.CacheType != CacheTypeEnum.Redis.ToString())
            return;

        // 若已开启集群配置，则把SignalR配置为支持集群模式
        var clusterOpt = App.GetConfig<ClusterOptions>("Cluster", true);
        if (!clusterOpt.Enabled)
            return;

        var redisOptions = clusterOpt.SentinelConfig;
        ConnectionMultiplexer connection1;
        if (clusterOpt.IsSentinel) // 哨兵模式
        {
            var redisConfig = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                ServiceName = redisOptions.ServiceName,
                AllowAdmin = true,
                DefaultDatabase = redisOptions.DefaultDb,
                Password = redisOptions.Password
            };
            redisOptions.EndPoints.ForEach(u => redisConfig.EndPoints.Add(u));
            connection1 = ConnectionMultiplexer.Connect(redisConfig);
        }
        else
        {
            connection1 = ConnectionMultiplexer.Connect(clusterOpt.SignalR.RedisConfiguration);
        }
        // 密钥存储（数据保护）
        services.AddDataProtection().PersistKeysToStackExchangeRedis(connection1, clusterOpt.DataProtecteKey);

        signalRBuilder.AddStackExchangeRedis(options =>
        {
            // 此处设置的ChannelPrefix并不会生效，如果两个不同的项目，且[程序集名+类名]一样，使用同一个redis服务，请注意修改 Hub/OnlineUserHub 的类名。
            // 原因请参考下边链接：
            // https://github.com/dotnet/aspnetcore/blob/f9121bc3e976ec40a959818451d126d5126ce868/src/SignalR/server/StackExchangeRedis/src/RedisHubLifetimeManager.cs#L74
            // https://github.com/dotnet/aspnetcore/blob/f9121bc3e976ec40a959818451d126d5126ce868/src/SignalR/server/StackExchangeRedis/src/Internal/RedisChannels.cs#L33
            options.Configuration.ChannelPrefix = new RedisChannel(clusterOpt.SignalR.ChannelPrefix, RedisChannel.PatternMode.Auto);
            options.ConnectionFactory = async writer =>
            {
                ConnectionMultiplexer connection;
                if (clusterOpt.IsSentinel)
                {
                    var config = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        ServiceName = redisOptions.ServiceName,
                        AllowAdmin = true,
                        DefaultDatabase = redisOptions.DefaultDb,
                        Password = redisOptions.Password
                    };
                    redisOptions.EndPoints.ForEach(u => config.EndPoints.Add(u));
                    connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                }
                else
                {
                    connection = await ConnectionMultiplexer.ConnectAsync(clusterOpt.SignalR.RedisConfiguration);
                }

                connection.ConnectionFailed += (_, e) =>
                {
                    "连接 Redis 失败".LogError();
                };

                if (!connection.IsConnected)
                {
                    "无法连接 Redis".LogError();
                }
                return connection;
            };
        });
    }
}