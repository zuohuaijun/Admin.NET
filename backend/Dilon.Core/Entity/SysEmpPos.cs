using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 员工职位表
    /// </summary>
    [Table("sys_emp_pos")]
    public class SysEmpPos : IEntity
    {
        /// <summary>
        /// 员工id
        /// </summary>
        public long SysEmpId { get; set; }

        /// <summary>
        /// 一对一引用（员工）
        /// </summary>
        public SysEmp SysEmp { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long SysPosId { get; set; }

        /// <summary>
        /// 一对一引用（职位）
        /// </summary>
        public SysPos SysPos { get; set; }
    }
}
