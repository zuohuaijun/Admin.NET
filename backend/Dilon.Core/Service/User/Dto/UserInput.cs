using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 用户参数
    /// </summary>
    public class UserPageInput : PageInputBase, IXnInputBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;

        public List<long> GrantMenuIdList { get; set; }
        public List<long> GrantRoleIdList { get; set; }
        public List<long> GrantOrgIdList { get; set; }
    }

    public class UserSelectorInput
    {
        public string Name { get; set; }
    }

    public class CreateUserInput : IXnInputBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;

        public List<long> GrantMenuIdList { get; set; }
        public List<long> GrantRoleIdList { get; set; }
        public List<long> GrantOrgIdList { get; set; }
    }

    public class AddUserInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号名称不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(Password), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;
    }

    public class CheckUserDataInput
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();
    }

    public class DeleteUserInput : BaseId
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();
    }

    public class UpdateUserInput : IXnInputBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号名称不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(Password), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;

        public List<long> GrantMenuIdList { get; set; }
        public List<long> GrantRoleIdList { get; set; }
        public List<long> GrantOrgIdList { get; set; }
    }

    public class QueryUserInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号名称不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空"), Compare(nameof(Password), ErrorMessage = "两次密码不一致")]
        public string Confirm { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public CommonStatus Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput2 SysEmpParam { get; set; } = new EmpOutput2();

        /// <summary>
        /// 搜索状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus SearchStatus { get; set; } = CommonStatus.ENABLE;
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