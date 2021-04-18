using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.Hosting;

namespace Dilon.Core
{
    /// <summary>
    /// 后台日志写入服务
    /// </summary>
    public class SimpleLogHostedService : IHostedService
    {
        private readonly IConcurrentQueue<SysLogEx> _logExQueue;
        private readonly IConcurrentQueue<SysLogOp> _logOpQueue;
        private readonly IConcurrentQueue<SysLogVis> _logVisQueue;
        private readonly ISysConfigService _sysConfigService;
        private readonly IRepository<SysLogEx> _sysLogExRepository;
        private readonly IRepository<SysLogOp> _sysLogOpRepository;
        private readonly IRepository<SysLogVis> _sysLogVisRepository;

        public SimpleLogHostedService(
            IConcurrentQueue<SysLogEx> logExQueue,
            IConcurrentQueue<SysLogOp> logOpQueue,
            IConcurrentQueue<SysLogVis> logVisQueue)
        {
            _logExQueue = logExQueue;
            _logOpQueue = logOpQueue;
            _logVisQueue = logVisQueue;
            _sysConfigService = App.GetService<ISysConfigService>();
            _sysLogExRepository = Db.GetRepository<SysLogEx>();
            _sysLogOpRepository = Db.GetRepository<SysLogOp>();
            _sysLogVisRepository = Db.GetRepository<SysLogVis>();
        }

        /// <summary>
        /// 工作线程
        /// </summary>
        private async Task DoWork()
        {
            // 日志暂存器，待写入
            List<SysLogEx> sysLogExs = new();
            List<SysLogOp> sysLogOps = new();
            List<SysLogVis> sysLogViss = new();
            while (true)
            {
                // 取系统配置，获得轮训间隔和单次写入容量
                (int interval, int quantity) = await _sysConfigService.GetLogWritingConfiguration();
                
                // 后台队列中产生了日志，取出写入暂存器
                int logExCount = _logExQueue.Count();
                if (logExCount > 0)
                {
                    for (int i = 0; i < logExCount; i++)
                    {
                        if (_logExQueue.Try(out SysLogEx obj))
                            sysLogExs.Add(obj);
                    }
                }

                int logOpCount = _logOpQueue.Count();
                if (logOpCount > 0)
                {
                    for (int i = 0; i < logOpCount; i++)
                    {
                        if (_logOpQueue.Try(out SysLogOp obj))
                            sysLogOps.Add(obj);
                    }
                }

                int logVisCount = _logVisQueue.Count();
                if (logVisCount > 0)
                {
                    for (int i = 0; i < logVisCount; i++)
                    {
                        if (_logVisQueue.Try(out SysLogVis obj))
                            sysLogViss.Add(obj);
                    }
                }

                // 达到系统配置的容量则写入数据库
                if (sysLogExs.Count > quantity)
                {
                    await _sysLogExRepository.InsertNowAsync(sysLogExs);
                    sysLogOps.Clear();
                }

                if (sysLogOps.Count > quantity)
                {
                    await _sysLogOpRepository.InsertNowAsync(sysLogOps);
                    sysLogOps.Clear();
                }

                if (sysLogViss.Count > quantity)
                {
                    await _sysLogVisRepository.InsertNowAsync(sysLogViss);
                    sysLogOps.Clear();
                }

                // 执行间隔
                await Task.Delay(interval);
            }
        }

        /// <summary>
        /// 任务开始
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(DoWork, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}