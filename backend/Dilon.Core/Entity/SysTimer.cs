using Furion.TaskScheduler;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [Table("sys_timer")]
    [Comment("定时任务表")]
    public class SysTimer : DEntityBase
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        /// <example>dilon</example>
        [Comment("任务名称")]
        [Required, MaxLength(20)]
        public string JobName { get; set; }

        /// <summary>
        /// 只执行一次
        /// </summary>
        [Comment("只执行一次")]
        public bool DoOnce { get; set; } = false;

        /// <summary>
        /// 立即执行（默认等待启动）
        /// </summary>
        [Comment("立即执行")]
        public bool StartNow { get; set; } = false;

        /// <summary>
        /// 执行类型(并行、列队)
        /// </summary>
        [Comment("执行类型")]
        public SpareTimeExecuteTypes ExecuteType { get; set; } = SpareTimeExecuteTypes.Parallel;

        /// <summary>
        /// 执行间隔时间（单位秒）
        /// </summary>
        /// <example>5</example>
        [Comment("间隔时间")]
        public int? Interval { get; set; } = 5;

        /// <summary>
        /// Cron表达式
        /// </summary>
        /// <example></example>
        [Comment("Cron表达式")]
        [MaxLength(20)]
        public string Cron { get; set; }

        /// <summary>
        /// 定时器类型
        /// </summary>
        [Comment("定时器类型")]
        public SpareTimeTypes TimerType { get; set; } = SpareTimeTypes.Interval;

        /// <summary>
        /// 请求url
        /// </summary>
        [Comment("请求url")]
        [MaxLength(200)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数（Post，Put请求用）
        /// </summary>
        [Comment("请求参数")]
        public string RequestParameters { get; set; }

        /// <summary>
        /// Headers(可以包含如：Authorization授权认证)
        /// 格式：{"Authorization":"userpassword.."}
        /// </summary>
        [Comment("Headers")]
        public string Headers { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// <example>2</example>
        [Comment("请求类型")]
        public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.Post;

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }
    }
}