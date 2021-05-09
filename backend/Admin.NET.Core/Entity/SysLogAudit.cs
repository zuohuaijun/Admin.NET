using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统操作/审计日志表
    /// </summary>
    [Table("sys_log_audit")]
    [Comment("审计日志表")]
    public class SysLogAudit : EntityBase
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Comment("表名")]
        [MaxLength(50)]
        public string TableName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        [Comment("列名")]
        [MaxLength(50)]
        public string ColumnName { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        [Comment("新值")]
        public string NewValue { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        [Comment("旧值")]
        public string OldValue { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Comment("操作时间")]
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        [Comment("操作人Id")]
        public long UserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [Comment("操作人名称")]
        [MaxLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 操作方式：新增、更新、删除
        /// </summary>
        [Comment("操作方式")]
        public DataOpType Operate { get; set; }
    }
}