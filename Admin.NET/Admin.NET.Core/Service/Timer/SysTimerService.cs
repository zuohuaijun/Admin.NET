namespace Admin.NET.Core.Service;

/// <summary>
/// 系统定时任务服务
/// </summary>
[ApiDescriptionSettings(Order = 188)]
public class SysTimerService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysTimer> _sysTimerRep;
    private readonly ISysCacheService _sysCacheService;

    public SysTimerService(SqlSugarRepository<SysTimer> sysTimerRep,
        ISysCacheService sysCacheService)
    {
        _sysTimerRep = sysTimerRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取任务分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysTimer/page")]
    public async Task<SqlSugarPagedList<TimerOutput>> GetTimerPage([FromQuery] PageTimerInput input)
    {
        var workers = SpareTime.GetWorkers().ToList();

        var timers = await _sysTimerRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.TimerName), u => u.TimerName.Contains(input.TimerName))
            .Select<TimerOutput>()
            .ToPagedListAsync(input.Page, input.PageSize);

        timers.Items.ToList().ForEach(u =>
        {
            var timer = workers.FirstOrDefault(m => m.WorkerName == u.TimerName);
            if (timer != null)
            {
                u.Status = timer.Status;
                u.Tally = timer.Tally;
                u.Exception = JSON.Serialize(timer.Exception);
            }
        });
        return timers;
    }

    /// <summary>
    /// 增加任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTimer/add")]
    public async Task AddTimer(AddTimerInput input)
    {
        var isExist = await _sysTimerRep.IsAnyAsync(u => u.TimerName == input.TimerName);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1100);

        var timer = input.Adapt<SysTimer>();
        await _sysTimerRep.InsertAsync(timer);

        CreateTimer(timer); // 添加到任务调度里
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTimer/delete")]
    public async Task DeleteTimer(DeleteTimerInput input)
    {
        var timer = await _sysTimerRep.GetFirstAsync(u => u.Id == input.Id);
        if (timer == null) throw Oops.Oh(ErrorCodeEnum.D1101);

        await _sysTimerRep.DeleteAsync(timer);

        SpareTime.Cancel(timer.TimerName); // 从调度器里取消
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTimer/update")]
    public async Task UpdateTimber(UpdateTimerInput input)
    {
        var isExist = await _sysTimerRep.IsAnyAsync(u => u.TimerName == input.TimerName && u.Id != input.Id);
        if (isExist) throw Oops.Oh(ErrorCodeEnum.D1100);

        // 先从调度器里取消
        var oldTimer = await _sysTimerRep.GetFirstAsync(u => u.Id == input.Id);
        SpareTime.Cancel(oldTimer.TimerName);

        var timer = input.Adapt<SysTimer>();
        await _sysTimerRep.AsUpdateable(timer).IgnoreColumns(true).ExecuteCommandAsync();

        CreateTimer(timer); // 再添加到任务调度里
    }

    /// <summary>
    /// 设置任务状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysTimer/setStatus")]
    public async void SetStatusTimer(SetTimerStatusInput input)
    {
        if (input.Status == SpareTimeStatus.Stopped)
            SpareTime.Stop(input.TimerName);
        else if (input.Status == SpareTimeStatus.Running)
        {
            var spareTime = SpareTime.GetWorkers().ToList().Find(u => u.WorkerName == input.TimerName);
            if (spareTime == null)
            {
                var timer = await _sysTimerRep.GetFirstAsync(u => u.TimerName == input.TimerName);
                CreateTimer(timer);
            }
            SpareTime.Start(input.TimerName); // 若StartNow=flase则不会启动任务
        }
    }

    /// <summary>
    /// 创建定时任务
    /// </summary>
    /// <param name="input"></param>
    private async void CreateTimer(SysTimer input)
    {
        Action<SpareTimer, long> action = null;
        switch (input.RequestType)
        {
            case RequestTypeEnum.Run: // 创建本地方法委托
                {
                    var taskMethod = GetTimerMethodList()?.Result.FirstOrDefault(m => m.RequestUrl == input.RequestUrl);
                    if (taskMethod == null) break;
                    var typeInstance = Activator.CreateInstance(taskMethod.DeclaringType);
                    action = (Action<SpareTimer, long>)Delegate.CreateDelegate(typeof(Action<SpareTimer, long>), typeInstance, taskMethod.MethodName);
                    break;
                }
            default: // 创建网络任务委托
                {
                    action = async (_, _) =>
                    {
                        var requestUrl = input.RequestUrl.Trim();
                        requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl; // 默认http协议
                        var requestParameters = input.RequestPara;
                        var headersString = input.Headers;
                        var headers = string.IsNullOrEmpty(headersString)
                            ? null : JSON.Deserialize<Dictionary<string, string>>(headersString);
                        switch (input.RequestType)
                        {
                            case RequestTypeEnum.Get:
                                await requestUrl.SetHeaders(headers).GetAsync();
                                break;

                            case RequestTypeEnum.Post:
                                await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PostAsync();
                                break;

                            case RequestTypeEnum.Put:
                                await requestUrl.SetHeaders(headers).SetQueries(requestParameters).PutAsync();
                                break;

                            case RequestTypeEnum.Delete:
                                await requestUrl.SetHeaders(headers).DeleteAsync();
                                break;
                        }
                    };
                    break;
                }
        }
        if (action == null) return;

        // 缓存任务配置参数供任务运行时读取
        if (input.RequestType == RequestTypeEnum.Run)
        {
            var timerParaName = $"{input.TimerName}_para";
            var timerPara = await _sysCacheService.ExistsAsync(timerParaName);
            var requestPara = string.IsNullOrEmpty(input.RequestPara);

            // 若没有任务配置但存在缓存则删除
            if (requestPara && timerPara)
                await _sysCacheService.RemoveAsync(timerParaName);
            else if (!requestPara)
                await _sysCacheService.SetAsync(timerParaName, JSON.Deserialize<Dictionary<string, string>>(input.RequestPara));
        }

        // 创建定时任务
        switch (input.TimerType)
        {
            case SpareTimeTypes.Interval:
                if (input.DoOnce)
                    SpareTime.DoOnce((int)input.Interval * 1000, action, input.TimerName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                else
                    SpareTime.Do((int)input.Interval * 1000, action, input.TimerName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                break;

            case SpareTimeTypes.Cron:
                SpareTime.Do(input.Cron, action, input.TimerName, input.Remark, input.StartNow, executeType: input.ExecuteType);
                break;
        }
    }

    /// <summary>
    /// 获取所有定时任务方法列表（贴spareTime特性）
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<TimerMethod>> GetTimerMethodList()
    {
        // 有缓存就返回缓存
        var timerMethodList = await _sysCacheService.GetAsync<IEnumerable<TimerMethod>>(CacheConst.KeyTimer);
        if (timerMethodList != null) return timerMethodList;

        timerMethodList = App.EffectiveTypes
            .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && typeof(ISpareTimeWorker).IsAssignableFrom(u))
            .SelectMany(u => u.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.IsDefined(typeof(SpareTimeAttribute), false) &&
                   m.GetParameters().Length == 2 &&
                   m.GetParameters()[0].ParameterType == typeof(SpareTimer) &&
                   m.GetParameters()[1].ParameterType == typeof(long) && m.ReturnType == typeof(void))
            .Select(m =>
            {
                // 默认获取第一条任务特性
                var spareTimeAttribute = m.GetCustomAttribute<SpareTimeAttribute>();
                return new TimerMethod
                {
                    TimerName = spareTimeAttribute.WorkerName,
                    RequestUrl = $"{m.DeclaringType.Name}/{m.Name}",
                    Cron = spareTimeAttribute.CronExpression,
                    DoOnce = spareTimeAttribute.DoOnce,
                    ExecuteType = spareTimeAttribute.ExecuteType,
                    Interval = (int)spareTimeAttribute.Interval / 1000,
                    StartNow = spareTimeAttribute.StartNow,
                    RequestType = RequestTypeEnum.Run,
                    Remark = spareTimeAttribute.Description,
                    TimerType = string.IsNullOrEmpty(spareTimeAttribute.CronExpression) ? SpareTimeTypes.Interval : SpareTimeTypes.Cron,
                    MethodName = m.Name,
                    DeclaringType = m.DeclaringType
                };
            }));

        await _sysCacheService.SetAsync(CacheConst.KeyTimer, timerMethodList);
        return timerMethodList;
    }

    /// <summary>
    /// 启动自启动任务
    /// </summary>
    [NonAction]
    public async void StartTimer()
    {
        var timerList = await _sysTimerRep.GetListAsync(t => t.StartNow);
        timerList.ForEach(CreateTimer);
    }
}