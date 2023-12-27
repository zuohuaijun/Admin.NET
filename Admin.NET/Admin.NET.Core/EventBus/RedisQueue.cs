// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using NewLife.Caching.Queues;

namespace Admin.NET.Core;

/// <summary>
/// Redis 消息队列
/// </summary>
public static class RedisQueue
{
    private static readonly ICache _cache = Cache.Default;

    /// <summary>
    /// 获取可信队列，需要确认
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static RedisReliableQueue<T> GetRedisReliableQueue<T>(string key)
    {
        var queue = (_cache as FullRedis).GetReliableQueue<T>(key);
        return queue;
    }

    /// <summary>
    /// 可信队列回滚
    /// </summary>
    /// <param name="key"></param>
    /// <param name="retryInterval"></param>
    /// <returns></returns>
    public static int RollbackAllAck(string key, int retryInterval = 60)
    {
        var queue = GetRedisReliableQueue<string>(key);
        queue.RetryInterval = retryInterval;
        return queue.RollbackAllAck();
    }

    /// <summary>
    /// 在可信队列获取一条数据
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReliableTakeOne<T>(string key)
    {
        var queue = GetRedisReliableQueue<T>(key);
        return queue.TakeOne(1);
    }

    /// <summary>
    /// 异步在可信队列获取一条数据
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> ReliableTakeOneAsync<T>(string key)
    {
        var queue = GetRedisReliableQueue<T>(key);
        return await queue.TakeOneAsync(1);
    }

    /// <summary>
    ///在可信队列获取多条数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="count"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> ReliableTake<T>(string key, int count)
    {
        var queue = GetRedisReliableQueue<T>(key);
        return queue.Take(count).ToList();
    }

    /// <summary>
    /// 发送一个数据列表到可信队列
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddReliableQueueList<T>(string key, List<T> value)
    {
        var queue = (_cache as FullRedis).GetReliableQueue<T>(key);
        var count = queue.Count;
        var result = queue.Add(value.ToArray());
        return result - count;
    }

    /// <summary>
    /// 发送一条数据到可信队列
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddReliableQueue<T>(string key, T value)
    {
        var queue = (_cache as FullRedis).GetReliableQueue<T>(key);
        var count = queue.Count;
        var result = queue.Add(value);
        return result - count;
    }

    /// <summary>
    /// 获取延迟队列
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static RedisDelayQueue<T> GetDelayQueue<T>(string key)
    {
        var queue = (_cache as FullRedis).GetDelayQueue<T>(key);
        return queue;
    }

    /// <summary>
    /// 发送一条数据到延迟队列
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="delay">延迟时间。单位秒</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int AddDelayQueue<T>(string key, T value, int delay)
    {
        var queue = GetDelayQueue<T>(key);
        return queue.Add(value, delay);
    }

    /// <summary>
    /// 发送数据列表到延迟队列
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="delay"></param>
    /// <typeparam name="T">延迟时间。单位秒</typeparam>
    /// <returns></returns>
    public static int AddDelayQueue<T>(string key, List<T> value, int delay)
    {
        var queue = GetDelayQueue<T>(key);
        queue.Delay = delay;
        return queue.Add(value.ToArray());
    }
}