// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

public class UserInput : BaseIdInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }
}

public class PageUserInput : BasePageInput
{
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 查询时所选机构Id
    /// </summary>
    public long OrgId { get; set; }
}

public class AddUserInput : SysUser
{
    /// <summary>
    /// 账号
    /// </summary>
    [Required(ErrorMessage = "账号不能为空")]
    public override string Account { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Required(ErrorMessage = "真实姓名不能为空")]
    public override string RealName { get; set; }

    /// <summary>
    /// 角色集合
    /// </summary>
    public List<long> RoleIdList { get; set; }

    /// <summary>
    /// 扩展机构集合
    /// </summary>
    public List<SysUserExtOrg> ExtOrgIdList { get; set; }
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
    [Required(ErrorMessage = "新密码不能为空")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "密码需要大于5个字符")]
    public string PasswordNew { get; set; }
}