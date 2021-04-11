using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 代码生成字段配置表
    /// </summary>
    [Table("sys_code_gen_config")]
    [Comment("代码生成字段配置表")]
    public class SysCodeGenConfig : DEntityBase
    {
        /// <summary>
        /// 代码生成主表ID
        /// </summary>
        [Comment("代码生成主表ID")]
        public long CodeGenId { get; set; }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        [Comment("数据库字段名")]
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [Comment("字段描述")]
        public string ColumnComment { get; set; }

        /// <summary>
        /// .NET数据类型
        /// </summary>
        [Comment(".NET数据类型")]
        public string NetType { get; set; }

        /// <summary>
        /// 作用类型（字典）
        /// </summary>
        [Comment("作用类型")]
        public string EffectType { get; set; }

        /// <summary>
        /// 字典code
        /// </summary>
        [Comment("字典Code")]
        public string DictTypeCode { get; set; }

        /// <summary>
        /// 列表是否缩进（字典）
        /// </summary>
        [Comment("列表是否缩进")]
        public string WhetherRetract { get; set; }

        /// <summary>
        /// 是否必填（字典）
        /// </summary>
        [Comment("是否必填")]
        public string WhetherRequired { get; set; }

        /// <summary>
        /// 是否是查询条件
        /// </summary>
        [Comment("是否是查询条件")]
        public string QueryWhether { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        [Comment("查询方式")]
        public string QueryType { get; set; }

        /// <summary>
        /// 列表显示
        /// </summary>
        [Comment("列表显示")]
        public string WhetherTable { get; set; }

        /// <summary>
        /// 增改
        /// </summary>
        [Comment("增改")]
        public string WhetherAddUpdate { get; set; }

        /// <summary>
        /// 主外键
        /// </summary>
        [Comment("主外键")]
        public string ColumnKey { get; set; }

        /// <summary>
        /// 数据库中类型（物理类型）
        /// </summary>
        [Comment("数据库中类型")]
        public string DataType { get; set; }

        /// <summary>
        /// 是否通用字段
        /// </summary>
        [Comment("是否通用字段")]
        public string WhetherCommon { get; set; }
    }
}
