using Furion.JsonSerialization;
using Furion.RemoteRequest.Extensions;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    [DisallowConcurrentExecution]
    public class HttpJob : IJob
    {
        //protected readonly int _maxLogCount = 20; //最多保存日志数量  
        //protected Stopwatch _stopwatch = new();

        public async Task Execute(IJobExecutionContext context)
        {
            // 获取相关参数
            var requestUrl = context.JobDetail.JobDataMap.GetString(SchedulerDef.REQUESTURL)?.Trim();
            requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
            var requestParameters = context.JobDetail.JobDataMap.GetString(SchedulerDef.REQUESTPARAMETERS);
            var headersString = context.JobDetail.JobDataMap.GetString(SchedulerDef.HEADERS);
            var headers = !string.IsNullOrWhiteSpace(headersString) ? JSON.GetJsonSerializer().Deserialize<Dictionary<string, string>>(headersString.Trim()) : null;
            var requestType = (RequestTypeEnum)int.Parse(context.JobDetail.JobDataMap.GetString(SchedulerDef.REQUESTTYPE));

            // var response = new HttpResponseMessage();
            switch (requestType)
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


            //_stopwatch.Restart(); // 开始监视代码运行时间
            //// var beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //Debug.WriteLine(DateTimeOffset.Now.ToString());

            //// var endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //_stopwatch.Stop(); // 停止监视

            ////// 执行次数
            ////var runNumber = context.JobDetail.JobDataMap.GetString(SchedulerDef.RUNNUMBER);
            ////context.JobDetail.JobDataMap[SchedulerDef.RUNNUMBER] = runNumber;

            //// 耗时
            //var seconds = _stopwatch.Elapsed.TotalSeconds;  // 总秒数             
            //var executeTime = seconds >= 1 ? seconds + "秒" : _stopwatch.Elapsed.TotalMilliseconds + "毫秒";

            ////// 只保留20条记录
            ////var logs = context.JobDetail.JobDataMap[SchedulerDef.LOGLIST] as List<string> ?? new List<string>();
            ////if (logs.Count >= _maxLogCount)
            ////    logs.RemoveRange(0, logs.Count - _maxLogCount);

            ////logs.Add($"<p class='msgList'><span class='time'>{beginTime} 至 {endTime}  【耗时】{executeTime}</span></p>");
            ////context.JobDetail.JobDataMap[SchedulerDef.LOGLIST] = logs;
        }
    }
}
