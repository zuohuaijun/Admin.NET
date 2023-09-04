using FluentEmail.Core;

namespace Admin.NET.Core;
public class RedisCache : FullRedis
{
    /// <summary>
    /// Redis前缀
    /// </summary>
    public string RedisPrefix { get; set; }

    /// <summary>实例化增强版Redis</summary>
    public RedisCache() : base() { }

    /// <summary>实例化增强版Redis</summary>
    /// <param name="server"></param>
    /// <param name="password"></param>
    /// <param name="db"></param>
    public RedisCache(String server, String password, Int32 db) : base(server, password, db) { }

    /// <summary>实例化增强版Redis</summary>
    /// <param name="options"></param>
    public RedisCache(RedisOptions options) :base(options) {
        RedisPrefix = options.Prefix;
    }

    /// <summary>按照配置服务实例化Redis，用于NETCore依赖注入</summary>
    /// <param name="provider">服务提供者，将要解析IConfigProvider</param>
    /// <param name="name">缓存名称，也是配置中心key</param>
    public RedisCache(IServiceProvider provider, String name) : base(provider, name) { }

    #region 基础操作
    /// <summary>缓存个数</summary>
    public override Int32 Count => base.Count;

    /// <summary>获取所有键，限制10000项，超额请使用FullRedis.Search</summary>
    public override ICollection<String> Keys
    {
        get
        {
            if (string.IsNullOrWhiteSpace(RedisPrefix))
            {
                return base.Keys;
            }
            else {
                return Execute(rds => rds.Execute<String[]>("KEYS", RedisPrefix + "*"));
            }
        }
    }

    /// <summary>获取信息</summary>
    /// <param name="all">是否获取全部信息，包括Commandstats</param>
    /// <returns></returns>
    public override IDictionary<String, String> GetInfo(Boolean all = false) => base.GetInfo(all);


    /// <summary>单个实体项</summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间，秒。小于0时采用默认缓存时间<seealso cref="Cache.Expire"/></param>
    public override Boolean Set<T>(String key, T value, Int32 expire = -1) => base.Set<T>(RedisPrefix+key,value,expire);

    /// <summary>获取单体</summary>
    /// <param name="key">键</param>
    public override T Get<T>(String key) => base.Get<T>(RedisPrefix + key);

    /// <summary>批量移除缓存项</summary>
    /// <param name="keys">键集合</param>
    public override Int32 Remove(params String[] keys) 
    {
        keys = keys.Select(key => RedisPrefix + key).ToArray();
        return base.Remove(keys);
    }

    /// <summary>清空所有缓存项</summary>
    public override void Clear() =>  base.Clear();

    /// <summary>是否存在</summary>
    /// <param name="key">键</param>
    public override Boolean ContainsKey(String key) => base.ContainsKey(RedisPrefix + key);

    /// <summary>设置缓存项有效期</summary>
    /// <param name="key">键</param>
    /// <param name="expire">过期时间</param>
    public override Boolean SetExpire(String key, TimeSpan expire) => base.SetExpire(RedisPrefix + key, expire);

    /// <summary>获取缓存项有效期</summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public override TimeSpan GetExpire(String key) => base.GetExpire(RedisPrefix + key);
    #endregion

    #region 集合操作
    /// <summary>批量获取缓存项</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keys"></param>
    /// <returns></returns>
    public override IDictionary<String, T> GetAll<T>(IEnumerable<String> keys) {
        keys = keys.Select(key => RedisPrefix + key).ToArray();
        return base.GetAll<T>(keys);
    }

    /// <summary>批量设置缓存项</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="expire">过期时间，秒。小于0时采用默认缓存时间<seealso cref="Cache.Expire"/></param>
    public override void SetAll<T>(IDictionary<String, T> values, Int32 expire = -1)
    {
        IDictionary<String, T> keyValues = new Dictionary<String, T>();
        values.ForEach(item =>
        {
            keyValues.Add(RedisPrefix + item.Key,item.Value);
        });
        base.SetAll(keyValues, expire);
    }

    /// <summary>获取哈希</summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public override IDictionary<String, T> GetDictionary<T>(String key) => base.GetDictionary<T>(RedisPrefix + key);

    /// <summary>获取队列</summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public override IProducerConsumer<T> GetQueue<T>(String key) =>  base.GetQueue<T>(RedisPrefix + key);

    /// <summary>获取栈</summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public override IProducerConsumer<T> GetStack<T>(String key) => base.GetStack<T>(RedisPrefix + key);

    /// <summary>获取Set</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public override ICollection<T> GetSet<T>(String key)  => base.GetSet<T>(RedisPrefix + key);
    #endregion

    #region 高级操作
    /// <summary>添加，已存在时不更新</summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="expire">过期时间，秒。小于0时采用默认缓存时间<seealso cref="Cache.Expire"/></param>
    /// <returns></returns>
    public override Boolean Add<T>(String key, T value, Int32 expire = -1) => base.Add<T>(RedisPrefix + key,value,expire);

    /// <summary>设置新值并获取旧值，原子操作</summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public override T Replace<T>(String key, T value) => base.Replace<T>(RedisPrefix + key, value);

    /// <summary>尝试获取指定键，返回是否包含值。有可能缓存项刚好是默认值，或者只是反序列化失败</summary>
    /// <remarks>
    /// 在 Redis 中，可能有key（此时TryGet返回true），但是因为反序列化失败，从而得不到value。
    /// </remarks>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">键</param>
    /// <param name="value">值。即使有值也不一定能够返回，可能缓存项刚好是默认值，或者只是反序列化失败</param>
    /// <returns>返回是否包含值，即使反序列化失败</returns>
    public override Boolean TryGetValue<T>(String key, out T value) => base.TryGetValue<T>(RedisPrefix + key,out value);

    /// <summary>累加，原子操作</summary>
    /// <param name="key">键</param>
    /// <param name="value">变化量</param>
    /// <returns></returns>
    public override Int64 Increment(String key, Int64 value) => base.Increment(RedisPrefix + key, value);

    /// <summary>累加，原子操作，乘以100后按整数操作</summary>
    /// <param name="key">键</param>
    /// <param name="value">变化量</param>
    /// <returns></returns>
    public override Double Increment(String key, Double value) => base.Increment(RedisPrefix + key, value);

    /// <summary>递减，原子操作</summary>
    /// <param name="key">键</param>
    /// <param name="value">变化量</param>
    /// <returns></returns>
    public override Int64 Decrement(String key, Int64 value) => base.Decrement(RedisPrefix + key, value);
    
    /// <summary>递减，原子操作，乘以100后按整数操作</summary>
    /// <param name="key">键</param>
    /// <param name="value">变化量</param>
    /// <returns></returns>
    public override Double Decrement(String key, Double value) => base.Decrement(RedisPrefix + key, value);
    #endregion
}
