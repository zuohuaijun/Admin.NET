using Furion.DatabaseAccessor;
using Furion.Snowflake;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("sys_role")]
    public class SysRole : DEntityBase, IEntityTypeBuilder<SysRole>
    {
        public SysRole()
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
        /// 数据范围类型（字典 1全部数据 2本部门及以下数据 3本部门数据 4仅本人数据 5自定义数据）
        /// </summary>
        public int DataScopeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 多对多（用户）
        /// </summary>
        public ICollection<SysUser> SysUsers { get; set; }

        /// <summary>
        /// 多对多中间表（用户角色）
        /// </summary>
        public List<SysUserRole> SysUserRoles { get; set; }

        /// <summary>
        /// 多对多（机构）
        /// </summary>
        public ICollection<SysOrg> SysOrgs { get; set; }

        /// <summary>
        /// 多对多中间表（角色-机构 数据范围）
        /// </summary>
        public List<SysRoleDataScope> SysRoleDataScopes { get; set; }

        /// <summary>
        /// 多对多（菜单）
        /// </summary>
        public ICollection<SysMenu> SysMenus { get; set; }

        /// <summary>
        /// 多对多中间表（角色-菜单）
        /// </summary>
        public List<SysRoleMenu> SysRoleMenus { get; set; }

        /// <summary>
        /// 配置多对多关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SysRole> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.SysOrgs).WithMany(p => p.SysRoles).UsingEntity<SysRoleDataScope>(
                u => u.HasOne(c => c.SysOrg).WithMany(c => c.SysRoleDataScopes).HasForeignKey(c => c.SysOrgId),
                u => u.HasOne(c => c.SysRole).WithMany(c => c.SysRoleDataScopes).HasForeignKey(c => c.SysRoleId),
                u =>
                {
                    u.HasKey(c => new { c.SysRoleId, c.SysOrgId });
                });

            entityBuilder.HasMany(p => p.SysMenus).WithMany(p => p.SysRoles).UsingEntity<SysRoleMenu>(
                u => u.HasOne(c => c.SysMenu).WithMany(c => c.SysRoleMenus).HasForeignKey(c => c.SysMenuId),
                u => u.HasOne(c => c.SysRole).WithMany(c => c.SysRoleMenus).HasForeignKey(c => c.SysRoleId),
                u =>
                {
                    u.HasKey(c => new { c.SysRoleId, c.SysMenuId });
                });
        }
    }
}
