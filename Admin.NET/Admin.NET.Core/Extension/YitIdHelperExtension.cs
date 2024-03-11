// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// YitIdHelper 自动获取WorkId拓展（支持分布式部署）
/// </summary>
public static class YitIdHelperExtension
{
    private const string MainLockName = "IdGen:WorkerId:Lock";
    private const string MainValueKey = "IdGen:WorkerId:Value";

    private static readonly List<string> _workIds = new();
    private static SnowIdOptions _options;

    public static void AddYitIdHelper(this IServiceCollection services, SnowIdOptions options)
    {
        _options = options;

        // 排除开发环境和Windows服务器
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || App.WebHostEnvironment.IsDevelopment())
        {
            YitIdHelper.SetIdGenerator(_options);
            return;
        }

        var maxLength = Math.Pow(2, _options.WorkerIdBitLength.ParseToDouble());
        for (int i = 0; i < maxLength; i++)
        {
            _workIds.Add(i.ToString());
        }

        Random ran = new();
        Thread.Sleep(ran.Next(10, 1000));

        SetWorkId();
    }

    private static void SetWorkId()
    {
        var lockName = $"{_options.WorkerPrefix}{MainLockName}";
        var valueKey = $"{_options.WorkerPrefix}{MainValueKey}";

        var minWorkId = 0;
        var maxWorkId = Math.Pow(2, _options.WorkerIdBitLength.ParseToDouble());

        var cache = App.GetService<ICache>();
        var redisLock = cache.AcquireLock(lockName, 10000, 15000, true);
        var keys = cache == Cache.Default
            ? cache.Keys.Where(u => u.StartsWith($"{_options.WorkerPrefix}{valueKey}:*"))
            : ((FullRedis)cache).Search($"{_options.WorkerPrefix}{valueKey}:*", int.MaxValue);

        var tempWorkIds = _workIds;
        foreach (var key in keys)
        {
            var tempWorkId = key[key.LastIndexOf(":", StringComparison.Ordinal)..];
            tempWorkIds.Remove(tempWorkId);
        }

        try
        {
            string workIdKey = "";
            foreach (var item in tempWorkIds)
            {
                var workIdStr = item;
                workIdKey = $"{valueKey}:{workIdStr}";
                var exist = cache.Get<bool>(workIdKey);
                if (exist)
                {
                    workIdKey = "";
                    continue;
                }

                Console.WriteLine($"###########当前应用WorkId:【{workIdStr}】###########");

                long workId = workIdStr.ParseToLong();
                if (workId < minWorkId || workId > maxWorkId)
                    continue;

                // 设置雪花Id算法机器码
                YitIdHelper.SetIdGenerator(new IdGeneratorOptions
                {
                    WorkerId = (ushort)workId,
                    WorkerIdBitLength = _options.WorkerIdBitLength,
                    SeqBitLength = _options.SeqBitLength
                });

                cache.Set(workIdKey, true, TimeSpan.FromSeconds(15));
                break;
            }

            if (string.IsNullOrWhiteSpace(workIdKey)) throw Oops.Oh("未设置有效的机器码,启动失败");

            // 开一个任务设置当前workId过期时间
            Task.Run(() =>
            {
                while (true)
                {
                    cache.SetExpire(workIdKey, TimeSpan.FromSeconds(15));
                    // Task.Delay(5000);
                    Thread.Sleep(10000);
                }
            });
        }
        catch (Exception ex)
        {
            throw Oops.Oh($"{ex.Message};{ex.StackTrace};{ex.StackTrace}");
        }
        finally
        {
            redisLock?.Dispose();
        }
    }
}