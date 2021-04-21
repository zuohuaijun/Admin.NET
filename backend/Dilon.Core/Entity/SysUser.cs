using Dilon.Core.Entity;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("sys_user")]
    [Comment("用户表")]
    public class SysUser : DBEntityTenant, IEntityTypeBuilder<SysUser>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Comment("账号")]
        [Required, MaxLength(20)]
        public string Account { get; set; }

        /// <summary>
        /// 密码（采用MD5加密）
        /// </summary>
        [Comment("密码")]
        [Required, MaxLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Comment("昵称")]
        [MaxLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Comment("姓名")]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Comment("头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Comment("生日")]
        public DateTimeOffset Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        [Comment("性别-男_1、女_2")]
        public Gender Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Comment("邮箱")]
        [MaxLength(20)]
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Comment("手机")]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        [MaxLength(20)]
        public string Tel { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Comment("最后登录IP")]
        [MaxLength(20)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Comment("最后登录时间")]
        public DateTimeOffset LastLoginTime { get; set; }

        /// <summary>
        /// 管理员类型-超级管理员_1、非管理员_2
        /// </summary>
        [Comment("管理员类型-超级管理员_1、非管理员_2")]
        public AdminType AdminType { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        [Comment("状态-正常_0、停用_1、删除_2")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 多对多（角色）
        /// </summary>
        public ICollection<SysRole> SysRoles { get; set; }

        /// <summary>
        /// 多对多中间表（用户-角色）
        /// </summary>
        public List<SysUserRole> SysUserRoles { get; set; }

        /// <summary>
        /// 多对多（机构）
        /// </summary>
        public ICollection<SysOrg> SysOrgs { get; set; }

        /// <summary>
        /// 多对多中间表（用户-机构 数据范围）
        /// </summary>
        public List<SysUserDataScope> SysUserDataScopes { get; set; }

        /// <summary>
        /// 配置多对多关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SysUser> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.SysRoles).WithMany(p => p.SysUsers).UsingEntity<SysUserRole>(
                u => u.HasOne(c => c.SysRole).WithMany(c => c.SysUserRoles).HasForeignKey(c => c.SysRoleId),
                u => u.HasOne(c => c.SysUser).WithMany(c => c.SysUserRoles).HasForeignKey(c => c.SysUserId),
                u =>
                {
                    u.HasKey(c => new { c.SysUserId, c.SysRoleId });
                });

            entityBuilder.HasMany(p => p.SysOrgs).WithMany(p => p.SysUsers).UsingEntity<SysUserDataScope>(
                u => u.HasOne(c => c.SysOrg).WithMany(c => c.SysUserDataScopes).HasForeignKey(c => c.SysOrgId),
                u => u.HasOne(c => c.SysUser).WithMany(c => c.SysUserDataScopes).HasForeignKey(c => c.SysUserId),
                u =>
                {
                    u.HasKey(c => new { c.SysUserId, c.SysOrgId });
                });
        }
    }
}
