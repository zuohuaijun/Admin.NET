using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class UserInput : BaseIdInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public virtual DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual int Sex { get; set; } = 1;

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual string IdCard { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public virtual string Signature { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public virtual string Introduction { get; set; }

        /// <summary>
        /// 账号类型-超级管理员_1、管理员_2、普通_3
        /// </summary>
        public virtual int UserType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public virtual long OrgId { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public virtual long PosId { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public virtual string JobNum { get; set; }

        /// <summary>
        /// 岗位状态
        /// </summary>
        public virtual int JobStatus { get; set; }
    }

    public class PageUserInput : BasePageInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }
    }

    public class AddUserInput : UserInput
    {
        /// <summary>
        /// 账号名称
        /// </summary>
        [Required(ErrorMessage = "账号名称不能为空")]
        public override string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "真实姓名不能为空")]
        public override string RealName { get; set; }

        ///// <summary>
        ///// 身份证号
        ///// </summary>
        //[Required(ErrorMessage = "身份证号不能为空")]
        //public override string IdCard { get; set; }
    }

    public class UpdateUserInput : AddUserInput
    {
    }

    public class DeleteUserInput : BaseIdInput
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }
    }

    public class ResetPwdUserInput : BaseIdInput
    {
    }

    public class ChangePwdInput
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required(ErrorMessage = "当前密码不能为空")]
        public string PasswordOld { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "原始密码不能为空")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
        public string PasswordNew { get; set; }
    }
}