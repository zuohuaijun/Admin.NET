using Quartz;
using System;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 任务信息---任务详情
    /// </summary>
    public class JobOutput
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 任务组名
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 上次执行的异常信息
        /// </summary>
        public string LastErrMsg { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TriggerState TriggerState { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string DisplayState
        {
            get
            {
                return TriggerState switch
                {
                    TriggerState.Normal => "正常",
                    TriggerState.Paused => "暂停",
                    TriggerState.Complete => "完成",
                    TriggerState.Error => "异常",
                    TriggerState.Blocked => "阻塞",
                    TriggerState.None => "不存在",
                    _ => "未知",
                };
            }
        }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 已经执行的次数
        /// </summary>
        public string RunNumber { get; set; }
    }
}
