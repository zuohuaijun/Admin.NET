using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
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
        [Required, MaxLength(100)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [Comment("字段描述")]
        [MaxLength(100)]
        public string ColumnComment { get; set; }

        /// <summary>
        /// .NET数据类型
        /// </summary>
        [Comment(".NET数据类型")]
        [MaxLength(50)]
        public string NetType { get; set; }

        /// <summary>
        /// 作用类型（字典）
        /// </summary>
        [Comment("作用类型")]
        [MaxLength(50)]
        public string EffectType { get; set; }

        /// <summary>
        /// 外键实体名称
        /// </summary>
        [Comment("外键实体名称")]
        [MaxLength(50)]
        public string FkEntityName { get; set; }

        /// <summary>
        /// 外键显示字段
        /// </summary>
        [Comment("外键显示字段")]
        [MaxLength(50)]
        public string FkColumnName { get; set; }

        /// <summary>
        /// 外键显示字段.NET类型
        /// </summary>
        [Comment("外键显示字段.NET类型")]
        [MaxLength(50)]
        public string FkColumnNetType { get; set; }

        /// <summary>
        /// 字典code
        /// </summary>
        [Comment("字典Code")]
        [MaxLength(50)]
        public string DictTypeCode { get; set; }

        /// <summary>
        /// 列表是否缩进（字典）
        /// </summary>
        [Comment("列表是否缩进")]
        [MaxLength(5)]
        public string WhetherRetract { get; set; }

        /// <summary>
        /// 是否必填（字典）
        /// </summary>
        [Comment("是否必填")]
        [MaxLength(5)]
        public string WhetherRequired { get; set; }

        /// <summary>
        /// 是否是查询条件
        /// </summary>
        [Comment("是否是查询条件")]
        [MaxLength(5)]
        public string QueryWhether { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        [Comment("查询方式")]
        [MaxLength(10)]
        public string QueryType { get; set; }

        /// <summary>
        /// 列表显示
        /// </summary>
        [Comment("列表显示")]
        [MaxLength(5)]
        public string WhetherTable { get; set; }

        /// <summary>
        /// 列表是否排序
        /// </summary>
        [Comment("列表是否排序")]
        [MaxLength(5)]
        public string WhetherOrderBy { get; set; }

        /// <summary>
        /// 增改
        /// </summary>
        [Comment("增改")]
        [MaxLength(5)]
        public string WhetherAddUpdate { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Comment("主键")]
        [MaxLength(5)]
        public string ColumnKey { get; set; }

        /// <summary>
        /// 数据库中类型（物理类型）
        /// </summary>
        [Comment("数据库中类型")]
        [MaxLength(50)]
        public string DataType { get; set; }

        /// <summary>
        /// 是否通用字段
        /// </summary>
        [Comment("是否通用字段")]
        [MaxLength(5)]
        public string WhetherCommon { get; set; }
    }
}