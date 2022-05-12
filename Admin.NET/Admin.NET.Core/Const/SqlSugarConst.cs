using SqlSugar;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// SqlSugar相关常量
    /// </summary>
    public class SqlSugarConst
    {
        /// <summary>
        /// 默认数据库标识
        /// </summary>
        public const string ConfigId = "Dilon";

        /// <summary>
        /// 默认表主键
        /// </summary>
        public const string PrimaryKey = "Id";

        /// <summary>
        /// SqlSugar数据库链接集合(多库代码生成用)
        /// </summary>
        public static List<ConnectionConfig> ConnectionConfigs = new List<ConnectionConfig>();
    }
}