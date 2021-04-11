using Dilon.Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
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
        public string JobName { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        /// <example>dilon</example>
        [Comment("任务分组")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Comment("开始时间")]
        public DateTimeOffset BeginTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <example>null</example>
        [Comment("结束时间")]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        /// <example></example>
        [Comment("Cron表达式")]
        public string Cron { get; set; }

        /// <summary>
        /// 执行次数（默认无限循环）
        /// </summary>
        /// <example>10</example>
        [Comment("执行次数")]
        public int? RunNumber { get; set; }

        /// <summary>
        /// 执行间隔时间，单位秒（如果有Cron，则IntervalSecond失效）
        /// </summary>
        /// <example>5</example>
        [Comment("执行间隔时间")]
        public int? Interval { get; set; } = 5;

        /// <summary>
        /// 触发器类型
        /// </summary>
        [Comment("触发器类型")]
        public TriggerTypeEnum TriggerType { get; set; } = TriggerTypeEnum.Simple;

        /// <summary>
        /// 请求url
        /// </summary>
        [Comment("请求url")]
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
        public string Remark { get; set; }
    }
}
