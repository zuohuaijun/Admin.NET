using Furion.DataValidation;
using Furion.TaskScheduler;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务调度参数
    /// </summary>
    public class JobPageInput : PageInputBase
    {
        /// <summary>
        /// 任务名称
        /// </summary>

        public string JobName { get; set; }

        /// <summary>
        /// 只执行一次
        /// </summary>
        public bool DoOnce { get; set; } = false;

        /// <summary>
        /// 立即执行（默认等待启动）
        /// </summary>
        public bool StartNow { get; set; } = false;

        /// <summary>
        /// 执行类型(并行、列队)
        /// </summary>
        public SpareTimeExecuteTypes ExecuteType { get; set; }

        /// <summary>
        /// 执行间隔时间（单位秒）
        /// </summary>
        /// <example>5</example>
        public int Interval { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        public SpareTimeTypes TimerType { get; set; }

        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数（Post，Put请求用）
        /// </summary>
        public string RequestParameters { get; set; }

        /// <summary>
        /// Headers(可以包含如：Authorization授权认证)
        /// 格式：{"Authorization":"userpassword.."}
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestTypeEnum RequestType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class AddJobInput
    {
        /// <summary>
        /// 任务名称
        /// </summary>

        public string JobName { get; set; }

        /// <summary>
        /// 只执行一次
        /// </summary>
        public bool DoOnce { get; set; } = false;

        /// <summary>
        /// 立即执行（默认等待启动）
        /// </summary>
        public bool StartNow { get; set; } = false;

        /// <summary>
        /// 执行类型(并行、列队)
        /// </summary>
        public SpareTimeExecuteTypes ExecuteType { get; set; }

        /// <summary>
        /// 执行间隔时间（单位秒）
        /// </summary>
        /// <example>5</example>
        public int Interval { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        public SpareTimeTypes TimerType { get; set; }

        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数（Post，Put请求用）
        /// </summary>
        public string RequestParameters { get; set; }

        /// <summary>
        /// Headers(可以包含如：Authorization授权认证)
        /// 格式：{"Authorization":"userpassword.."}
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestTypeEnum RequestType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class StopJobInput
    {
        public string JobName { get; set; }
    }

    public class DeleteJobInput : BaseId
    {
    }

    public class UpdateJobInput : BaseId
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        /// <example>dilon</example>
        [Required, MaxLength(20)]
        public string JobName { get; set; }

        /// <summary>
        /// 只执行一次
        /// </summary>
        public bool DoOnce { get; set; } = false;

        /// <summary>
        /// 立即执行（默认等待启动）
        /// </summary>
        public bool StartNow { get; set; } = false;

        /// <summary>
        /// 执行类型(并行、列队)
        /// </summary>
        public SpareTimeExecuteTypes ExecuteType { get; set; } = SpareTimeExecuteTypes.Parallel;

        /// <summary>
        /// 执行间隔时间（单位秒）
        /// </summary>
        /// <example>5</example>
        public int? Interval { get; set; } = 5;

        /// <summary>
        /// Cron表达式
        /// </summary>
        /// <example></example>
        [MaxLength(20)]
        public string Cron { get; set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        public SpareTimeTypes TimerType { get; set; } = SpareTimeTypes.Interval;

        /// <summary>
        /// 请求url
        /// </summary>
        [MaxLength(200)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数（Post，Put请求用）
        /// </summary>
        public string RequestParameters { get; set; }

        /// <summary>
        /// Headers(可以包含如：Authorization授权认证)
        /// 格式：{"Authorization":"userpassword.."}
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// <example>2</example>
        public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.Post;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public string Remark { get; set; }
    }

    public class QueryJobInput : BaseId
    {
    }
}