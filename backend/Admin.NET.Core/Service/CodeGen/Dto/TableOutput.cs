namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 数据库表列表参数
    /// </summary>
    public class TableOutput
    {
        /// <summary>
        /// 表名（字母形式的）
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 表名称描述（注释）（功能名）
        /// </summary>
        public string TableComment { get; set; }
    }
}