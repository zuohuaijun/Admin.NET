using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 员工职位表
    /// </summary>
    [Table("sys_emp_pos")]
    [Comment("员工职位表")]
    public class SysEmpPos : IEntity
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        [Comment("员工Id")]
        public long SysEmpId { get; set; }

        /// <summary>
        /// 一对一引用（员工）
        /// </summary>
        public SysEmp SysEmp { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        [Comment("职位Id")]
        public long SysPosId { get; set; }

        /// <summary>
        /// 一对一引用（职位）
        /// </summary>
        public SysPos SysPos { get; set; }
    }
}