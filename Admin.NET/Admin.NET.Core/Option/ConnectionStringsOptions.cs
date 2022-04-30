using Furion.ConfigurableOptions;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 数据库链接配置
    /// </summary>
    public class ConnectionStringsOptions : IConfigurableOptions
    {
        /// <summary>
        /// 默认数据库编号
        /// </summary>
        public string DefaultConfigId { get; set; } = SqlSugarConst.ConfigId;

        /// <summary>
        /// 默认数据库类型
        /// </summary>
        public string DefaultDbType { get; set; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>

        public string DefaultConnection { get; set; }

        /// <summary>
        /// 初始化表和数据
        /// </summary>
        public bool InitTable { get; set; }

        /// <summary>
        /// 业务库集合
        /// </summary>
        public List<DbConfig> DbConfigs { get; set; } = new List<DbConfig>();
    }

    /// <summary>
    /// 数据库参数
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string DbConfigId { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnection { get; set; }

        /// <summary>
        /// 初始化表和数据
        /// </summary>
        public bool InitTable { get; set; }
    }
}
