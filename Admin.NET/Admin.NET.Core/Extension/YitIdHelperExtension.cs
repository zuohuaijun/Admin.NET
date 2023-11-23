//// 麻省理工学院许可证
////
//// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
////
//// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
////
//// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
//// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// YitIdHelper 自动获取WorkId拓展
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
        int milliseconds = ran.Next(10, 1000);
        Thread.Sleep(milliseconds);

        SetWorkId();
    }

    private static void SetWorkId()
    {
        var lockName = $"{_options.WorkerPrefix}{MainLockName}";
        var valueKey = $"{_options.WorkerPrefix}{MainValueKey}";

        var minWorkId = 0;
        var maxWorkId = Math.Pow(2, _options.WorkerIdBitLength.ParseToDouble());

        var client = App.GetService<ICache>();
        var redisLock = client.AcquireLock(lockName, 10000, 15000, true);
        var keys = client.Keys.Where(o => o.Contains($"{_options.WorkerPrefix}{valueKey}:*"));

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
                string workIdStr = "";

                workIdStr = item;
                workIdKey = $"{valueKey}:{workIdStr}";
                var exist = client.Get<bool>(workIdKey);

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

                client.Set(workIdKey, true, TimeSpan.FromSeconds(15));

                break;
            }

            if (string.IsNullOrWhiteSpace(workIdKey)) throw Oops.Oh("未设置有效的机器码,启动失败");

            // 开一个任务设置当前workId过期时间
            Task.Run(() =>
            {
                while (true)
                {
                    client.SetExpire(workIdKey, TimeSpan.FromSeconds(15));
                    //Task.Delay(5000);
                    Thread.Sleep(10000);
                }
            });
        }
        catch (Exception e)
        {
            throw Oops.Oh($"{e.Message};{e.StackTrace};{e.StackTrace}");
        }
        finally
        {
            redisLock?.Dispose();
        }
    }
}