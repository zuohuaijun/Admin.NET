using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [SugarTable("sys_user", "系统用户表")]
    [SqlSugarEntity]
    public class SysUser : EntityTenant
    {
        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(ColumnDescription = "账号名称", Length = 20)]
        [Required, MaxLength(20)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        [SugarColumn(ColumnDescription = "账号密码", Length = 50)]
        [Required, MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "昵称", Length = 20)]
        [MaxLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(ColumnDescription = "头像", Length = 255)]
        [MaxLength(255)]
        public string Avatar { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [SugarColumn(ColumnDescription = "出生日期")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        [SugarColumn(ColumnDescription = "性别")]
        public GenderEnum Sex { get; set; } = GenderEnum.Male;

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnDescription = "邮箱", Length = 50)]
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(ColumnDescription = "手机号码", Length = 20)]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [SugarColumn(ColumnDescription = "真实姓名", Length = 20)]
        [MaxLength(20)]
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(ColumnDescription = "身份证号", Length = 20)]
        [MaxLength(20)]
        public string IdCard { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [SugarColumn(ColumnDescription = "个性签名", Length = 50)]
        [MaxLength(50)]
        public string Signature { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [SugarColumn(ColumnDescription = "个人简介", Length = 500)]
        [MaxLength(500)]
        public string Introduction { get; set; }

        /// <summary>
        /// 账号类型-超级管理员_1、管理员_2、普通_3
        /// </summary>
        [SugarColumn(ColumnDescription = "账号类型")]
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.None;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注", Length = 100)]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;

        /// <summary>
        /// 机构Id
        /// </summary>
        [SugarColumn(ColumnDescription = "机构Id")]
        public long OrgId { get; set; }

        /// <summary>
        /// 机构
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysOrg SysOrg { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        [SugarColumn(ColumnDescription = "职位Id")]
        public long PosId { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public SysPos SysPos { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(ColumnDescription = "工号", Length = 30)]
        [MaxLength(30)]
        public string JobNum { get; set; }

        /// <summary>
        /// 岗位状态
        /// </summary>
        [SugarColumn(ColumnDescription = "岗位状态")]
        public JobStatusEnum JobStatus { get; set; } = JobStatusEnum.On;
    }
}