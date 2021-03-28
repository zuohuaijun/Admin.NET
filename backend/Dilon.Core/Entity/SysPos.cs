using Furion.Snowflake;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table("sys_pos")]
    public class SysPos : DEntityBase
    {
        public SysPos()
        {
            Id = IDGenerator.NextId();
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Status = (int)CommonStatus.ENABLE;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }

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
