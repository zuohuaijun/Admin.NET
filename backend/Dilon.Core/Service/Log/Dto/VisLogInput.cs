using System;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 访问日志参数
    /// </summary>
    public class VisLogInput : PageInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否执行成功（Y-是，N-否）
        /// </summary>
        public string Success { get; set; }

        /// <summary>
        /// 具体消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 访问类型（字典 1登入 2登出）
        /// </summary>
        public int? VisType { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTimeOffset VisTime { get; set; }

        /// <summary>
        /// 访问人
        /// </summary>
        public string Account { get; set; }
    }
}
