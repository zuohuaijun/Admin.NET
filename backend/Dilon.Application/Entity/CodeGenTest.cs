using Dilon.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Application
{
    /// <summary>
    /// 代码生成实体测试（EF）
    /// </summary>
    [Table("code_gen_test")]
    [Comment("代码生成业务")]
    public class CodeGenTest : DEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Comment("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Comment("生日")]
        public DateTimeOffset Birthday { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Comment("年龄")]
        public int Age { get; set; }
    }
}
