using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table("sys_pos")]
    [Comment("职位表")]
    public class SysPos : DEntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 多对多（员工）
        /// </summary>
        public ICollection<SysEmp> SysEmps { get; set; }

        /// <summary>
        /// 多对多中间表（员工职位）
        /// </summary>
        public List<SysEmpPos> SysEmpPos { get; set; }
    }
}
