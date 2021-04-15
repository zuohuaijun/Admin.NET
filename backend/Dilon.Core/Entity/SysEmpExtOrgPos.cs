using Dilon.Core.Entity;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 员工附属机构职位表
    /// </summary>
    [Table("sys_emp_ext_org_pos")]
    [Comment("员工附属机构职位表")]
    public class SysEmpExtOrgPos : IEntity, IEntityTypeBuilder<SysEmpExtOrgPos>, IEntitySeedData<SysEmpExtOrgPos>
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
        /// 机构Id
        /// </summary>
        [Comment("机构Id")]
        public long SysOrgId { get; set; }

        /// <summary>
        /// 一对一引用（机构）
        /// </summary>
        public SysOrg SysOrg { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        [Comment("职位Id")]
        public long SysPosId { get; set; }

        /// <summary>
        /// 一对一引用（职位）
        /// </summary>
        public SysPos SysPos { get; set; }

        public void Configure(EntityTypeBuilder<SysEmpExtOrgPos> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasKey(c => new { c.SysEmpId, c.SysOrgId, c.SysPosId });
        }

        public IEnumerable<SysEmpExtOrgPos> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910539, SysPosId = 142307070910547 },
                new SysEmpExtOrgPos {SysEmpId = 142307070910551, SysOrgId = 142307070910540, SysPosId = 142307070910548 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910541, SysPosId = 142307070910549 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910551, SysOrgId = 142307070910542, SysPosId = 142307070910550 },
                new SysEmpExtOrgPos { SysEmpId = 142307070910553, SysOrgId = 142307070910542, SysPosId = 142307070910547 }
            };
        }
    }
}
