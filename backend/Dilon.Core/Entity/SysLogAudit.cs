using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 系统操作/审计日志表
    /// </summary>
    [Table("sys_log_audit")]
    public class SysLogAudit : EntityBase
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作方式：新增、更新、删除
        /// </summary>
        public string Operate { get; set; }
    }
}
