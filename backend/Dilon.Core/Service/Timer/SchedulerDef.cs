namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务调度相关常量
    /// </summary>
    public class SchedulerDef
    {
        /// <summary>
        /// 请求url RequestUrl
        /// </summary>
        public const string REQUESTURL = "RequestUrl";
        /// <summary>
        /// 请求参数 RequestParameters
        /// </summary>
        public const string REQUESTPARAMETERS = "RequestParameters";
        /// <summary>
        /// Headers（可以包含：Authorization授权认证）
        /// </summary>
        public const string HEADERS = "Headers";
        /// <summary>
        /// 请求类型 RequestType
        /// </summary>
        public const string REQUESTTYPE = "RequestType";
        /// <summary>
        /// 日志 LogList
        /// </summary>
        public const string LOGLIST = "LogList";
        /// <summary>
        /// 异常 Exception
        /// </summary>
        public const string EXCEPTION = "Exception";
        /// <summary>
        /// 执行次数
        /// </summary>
        public const string RUNNUMBER = "RunNumber";
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public const string ENDAT = "EndAt";
    }

    /// <summary>
    /// http请求类型
    /// </summary>
    public enum RequestTypeEnum
    {
        None = 0,
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }

    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum TriggerTypeEnum
    {
        None = 0,
        Simple = 1,
        Cron = 2
    }
}
