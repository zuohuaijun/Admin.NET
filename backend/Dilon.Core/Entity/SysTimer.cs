using Dilon.Core.Service;
using Furion.Snowflake;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [Table("sys_timer")]
    public class SysTimer : DEntityBase
    {
        public SysTimer()
        {
            Id = IDGenerator.NextId();
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        /// <example>dilon</example>
        public string JobName { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        /// <example>dilon</example>
        public string JobGroup { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset BeginTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <example>null</example>
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        /// <example></example>
        public string Cron { get; set; }

        /// <summary>
        /// 执行次数（默认无限循环）
        /// </summary>
        /// <example>10</example>
        public int? RunNumber { get; set; }

        /// <summary>
        /// 执行间隔时间，单位秒（如果有Cron，则IntervalSecond失效）
        /// </summary>
        /// <example>5</example>
        public int? Interval { get; set; } = 5;

        /// <summary>
        /// 触发器类型
        /// </summary>
        public TriggerTypeEnum TriggerType { get; set; } = TriggerTypeEnum.Simple;

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
        /// <example>2</example>
        public RequestTypeEnum RequestType { get; set; } = RequestTypeEnum.Post;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}
