using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 代码生成表
    /// </summary>
    [Table("sys_code_gen")]
    [Comment("代码生成表")]
    public class SysCodeGen : DEntityBase
    {
        /// <summary>
        /// 作者姓名
        /// </summary>
        [Comment("作者姓名")]
        public string AuthorName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        [Comment("是否移除表前缀")]
        public string TablePrefix { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        [Comment("生成方式")]
        public string GenerateType { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        [Comment("数据库表名")]
        public string TableName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Comment("命名空间")]
        public string NameSpace { get; set; }

        /// <summary>
        /// 业务名
        /// </summary>
        [Comment("业务名")]
        public string BusName { get; set; }
    }
}
