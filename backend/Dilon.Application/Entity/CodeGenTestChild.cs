using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dilon.Application;
using Microsoft.EntityFrameworkCore;

namespace Dilon.Core
{
    /// <summary>
    /// 代码生成测试子表
    /// </summary>
    [Table("code_gen_test_child")]
    [Comment("代码生成测试子表")]
    public class CodeGenTestChild : DEntityBase
    {
        [ForeignKey("CodeGenId")]
        public virtual CodeGenTest CodeGen { get; set; }
        
        /// <summary>
        /// 主表外键
        /// </summary>
        [Comment("主")]
        public virtual long CodeGenId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [MaxLength(32)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [MaxLength(32)]
        public virtual string Name { get; set; }
    }
}