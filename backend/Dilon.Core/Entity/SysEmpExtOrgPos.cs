using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 员工附属机构职位表
    /// </summary>
    [Table("sys_emp_ext_org_pos")]
    public class SysEmpExtOrgPos : EntityBase
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
        /// 机构Id
        /// </summary>
        public long SysOrgId { get; set; }

        /// <summary>
        /// 一对一引用（机构）
        /// </summary>
        public SysOrg SysOrg { get; set; }

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
