using System;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 异常日志参数
    /// </summary>
    public class ExLogOutput
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
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTimeOffset ExceptionTime { get; set; }
    }
}