// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统OAuth服务
/// </summary>
[AllowAnonymous]
[ApiDescriptionSettings(Order = 100)]
public class SysOAuthService : IDynamicApiController, ITransient
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;
    private readonly SysUserService _sysUserService;

    public SysOAuthService(IHttpContextAccessor httpContextAccessor,
        SqlSugarRepository<SysWechatUser> sysWechatUserRep,
        SysUserService sysUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _sysWechatUserRep = sysWechatUserRep;
        _sysUserService = sysUserService;
    }

    /// <summary>
    /// 第三方登录
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="redirectUrl"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SignIn"), HttpGet]
    [DisplayName("第三方登录")]
    public async Task<IActionResult> SignIn([FromQuery] string provider, [FromQuery] string redirectUrl)
    {
        if (string.IsNullOrWhiteSpace(provider) || !await _httpContextAccessor.HttpContext.IsProviderSupportedAsync(provider))
            throw Oops.Oh("不支持的OAuth类型");

        var request = _httpContextAccessor.HttpContext.Request;
        var url = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}Callback?provider={provider}&redirectUrl={redirectUrl}";
        var properties = new AuthenticationProperties { RedirectUri = url };
        properties.Items["LoginProvider"] = provider;
        return await Task.FromResult(new ChallengeResult(provider, properties));
    }

    /// <summary>
    /// 授权回调
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="redirectUrl"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "SignInCallback"), HttpGet]
    [DisplayName("授权回调")]
    public async Task<IActionResult> SignInCallback([FromQuery] string provider = null, [FromQuery] string redirectUrl = "")
    {
        if (string.IsNullOrWhiteSpace(provider) || !await _httpContextAccessor.HttpContext.IsProviderSupportedAsync(provider))
            throw Oops.Oh("不支持的OAuth类型");

        var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(provider);
        if (!authenticateResult.Succeeded)
            throw Oops.Oh("授权失败");

        var openIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
        if (openIdClaim == null || string.IsNullOrWhiteSpace(openIdClaim.Value))
            throw Oops.Oh("授权失败");

        if (provider == "Weixin")
        {
        }
        else if (provider == "Gitee")
        {
            string email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            string name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
            string giteeName = authenticateResult.Principal.FindFirst(GiteeClaims.Name)?.Value;
            string avatarUrl = authenticateResult.Principal.FindFirst(GiteeClaims.AvatarUrl)?.Value;

            // 若账号不存在则新建
            var user = await _sysWechatUserRep.GetFirstAsync(u => u.OpenId == openIdClaim.Value);
            if (user == null)
            {
                var userId = await _sysUserService.AddUser(new AddUserInput()
                {
                    Account = name,
                    RealName = name,
                    NickName = name,
                    Email = email,
                    Avatar = avatarUrl,
                    Phone = "",
                });

                user = await _sysWechatUserRep.InsertReturnEntityAsync(new SysWechatUser()
                {
                    UserId = userId,
                    OpenId = openIdClaim.Value,
                    Avatar = avatarUrl,
                    NickName = name,
                });
            }

            // 构建登录Token


        }

        return new RedirectResult($"{redirectUrl}?openId={openIdClaim.Value}");
    }
}