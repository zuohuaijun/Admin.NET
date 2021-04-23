using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace Dilon.Core
{
    /// <summary>
    /// 异常日志
    /// </summary>
    [Table("sys_log_ex")]
    [Comment("异常日志表")]
    public class SysLogEx : EntityBase
    {
        /// <summary>
        /// 操作人
        /// </summary>
        [Comment("操作人")]
        [MaxLength(20)]
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        [Comment("类名")]
        [MaxLength(100)]
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        [Comment("方法名")]
        [MaxLength(100)]
        public string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        [Comment("异常名称")]
        public string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [MaxLength(2000)]
        [Comment("异常信息")]
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        [MaxLength(2000)]
        [Comment("异常源")]
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        [MaxLength(5000)]
        [Comment("堆栈信息")]
        public string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        [MaxLength(5000)]
        [Comment("参数对象")]
        public string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [Comment("异常时间")]
        public DateTimeOffset ExceptionTime { get; set; }
    }
}