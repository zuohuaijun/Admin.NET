using System;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 用户参数
    /// </summary>
    public class UserInput : XnInputBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public virtual int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public virtual CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;
    }

    public class AddUserInput : UserInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号名称不能为空")]
        public override string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public override string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(Password), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }
    }

    public class DeleteUserInput : UserInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateUserInput : UserInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }
    }

    public class QueryUserInput : UpdateUserInput
    {
    }

    public class ChangePasswordUserInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(NewPassword), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }
    }

    public class UploadAvatarInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 头像文件路径标识
        /// </summary>
        [Required(ErrorMessage = "头像文件路径标识不能为空")]
        public long Avatar { get; set; }
    }
}