using System.Collections.Generic;

namespace Admin.NET.Core.Service
{
    public class XnCodeGenOutput
    {
        /// <summary>
        /// 作者姓名
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        public string TablePrefix { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        public string GenerateType { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据库表名（经过组装的）
        /// </summary>
        public string TableNameAss { get; set; }

        /// <summary>
        /// 代码包名
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 生成时间（string类型的）
        /// </summary>
        public string CreateTimestring { get; set; }

        /// <summary>
        /// 数据库表中字段集合
        /// </summary>
        public List<SysCodeGenConfig> ConfigList { get; set; }

        /// <summary>
        /// 业务名
        /// </summary>
        public string BusName { get; set; }
    }
}