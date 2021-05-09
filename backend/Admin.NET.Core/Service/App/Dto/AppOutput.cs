namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 系统应用参数
    /// </summary>
    public class AppOutput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}