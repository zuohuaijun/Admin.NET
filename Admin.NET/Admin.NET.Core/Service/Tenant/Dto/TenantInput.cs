// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

public class TenantInput : BaseIdInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }
}

public class PageTenantInput : BasePageInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public virtual string Phone { get; set; }
}

public class AddTenantInput : TenantOutput
{
    /// <summary>
    /// 租户名称
    /// </summary>
    [Required(ErrorMessage = "租户名称不能为空"), MinLength(2, ErrorMessage = "租户名称不能少于2个字符")]
    public override string Name { get; set; }

    /// <summary>
    /// 租管账号
    /// </summary>
    [Required(ErrorMessage = "租管账号不能为空"), MinLength(3, ErrorMessage = "租管账号不能少于3个字符")]
    public override string AdminAccount { get; set; }
}

public class UpdateTenantInput : AddTenantInput
{
}

public class DeleteTenantInput : BaseIdInput
{
}

public class TenantUserInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }
}

public class TenantIdInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }
}