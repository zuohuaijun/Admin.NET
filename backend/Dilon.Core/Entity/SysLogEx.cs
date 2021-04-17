using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;

namespace Dilon.Core
{
    /// <summary>
    /// 异常日志
    /// </summary>
    [Table("sys_log_ex")]
    public class SysLogEx : EntityBase
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string Account { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string ExceptionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [MaxLength(2000)]
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        [MaxLength(2000)]
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        [MaxLength(5000)]
        public string StackTrace { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        [MaxLength(5000)]
        public string ParamsObj { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTimeOffset ExceptionTime { get; set; }
    }
}