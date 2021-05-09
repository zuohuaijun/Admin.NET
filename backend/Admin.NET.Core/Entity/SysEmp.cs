using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 员工表
    /// </summary>
    [Table("sys_emp")]
    [Comment("员工表")]
    public class SysEmp : IEntity, IEntityTypeBuilder<SysEmp>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("用户Id")]
        public long Id { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Comment("工号")]
        [MaxLength(30)]
        public string JobNum { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        [Comment("机构Id")]
        public long OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Comment("机构名称")]
        [MaxLength(50)]
        public string OrgName { get; set; }

        /// <summary>
        /// 多对多（职位）
        /// </summary>
        public ICollection<SysPos> SysPos { get; set; }

        /// <summary>
        /// 多对多中间表（员工-职位）
        /// </summary>
        public List<SysEmpPos> SysEmpPos { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SysEmp> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.SysPos).WithMany(p => p.SysEmps).UsingEntity<SysEmpPos>(
                u => u.HasOne(c => c.SysPos).WithMany(c => c.SysEmpPos).HasForeignKey(c => c.SysPosId),
                u => u.HasOne(c => c.SysEmp).WithMany(c => c.SysEmpPos).HasForeignKey(c => c.SysEmpId),
                u =>
                {
                    u.HasKey(c => new { c.SysEmpId, c.SysPosId });
                });
        }
    }
}