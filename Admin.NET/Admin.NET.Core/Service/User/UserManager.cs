// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 当前登录用户
/// </summary>
public class UserManager : IScoped
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId => (_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.UserId)?.Value).ToLong();

    /// <summary>
    /// 租户ID
    /// </summary>
    public long TenantId => (_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.TenantId)?.Value).ToLong();

    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.Account)?.Value;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.RealName)?.Value;

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    public bool SuperAdmin => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString();

    /// <summary>
    /// 组织机构Id
    /// </summary>
    public long OrgId => (_httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OrgId)?.Value).ToLong();

    /// <summary>
    /// 微信OpenId
    /// </summary>
    public string OpenId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimConst.OpenId)?.Value;

    public UserManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}