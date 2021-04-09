using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 代码生成表
    /// </summary>
    [Table("sys_code_gen")]
    public class SysCodeGen : DEntityBase
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
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 业务名
        /// </summary>
        public string BusName { get; set; }
    }
}
