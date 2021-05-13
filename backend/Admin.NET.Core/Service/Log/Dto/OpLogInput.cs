namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 请求日志参数
    /// </summary>
    public class OpLogPageInput : PageInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否执行成功（Y-是，N-否）
        /// </summary>
        public YesOrNot Success { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string ReqMethod { get; set; }
    }
}