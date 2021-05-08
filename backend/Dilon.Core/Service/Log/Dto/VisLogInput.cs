namespace Dilon.Core.Service
{
    /// <summary>
    /// 访问日志参数
    /// </summary>
    public class VisLogPageInput : PageInputBase
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
        /// 访问类型
        /// </summary>
        public LoginType VisType { get; set; }
    }
}