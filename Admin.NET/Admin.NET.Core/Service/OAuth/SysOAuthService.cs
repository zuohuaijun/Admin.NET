// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统OAuth服务
/// </summary>
[AllowAnonymous]
[ApiDescriptionSettings(Order = 495)]
public class SysOAuthService : IDynamicApiController, ITransient
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;

    public SysOAuthService(IHttpContextAccessor httpContextAccessor,
        SqlSugarRepository<SysWechatUser> sysWechatUserRep)
    {
        _httpContextAccessor = httpContextAccessor;
        _sysWechatUserRep = sysWechatUserRep;
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

        var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
        var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var mobilePhone = authenticateResult.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var dateOfBirth = authenticateResult.Principal.FindFirst(ClaimTypes.DateOfBirth)?.Value;
        var gender = authenticateResult.Principal.FindFirst(ClaimTypes.Gender)?.Value;
        var avatarUrl = "";

        var platformType = PlatformTypeEnum.微信公众号;
        if (provider == "Gitee")
        {
            platformType = PlatformTypeEnum.Gitee;
            avatarUrl = authenticateResult.Principal.FindFirst(OAuthClaim.GiteeAvatarUrl)?.Value;
        }

        // 若账号不存在则新建
        var wechatUser = await _sysWechatUserRep.AsQueryable().Includes(u => u.SysUser).ClearFilter().FirstAsync(u => u.OpenId == openIdClaim.Value);
        if (wechatUser == null)
        {
            var userId = await App.GetRequiredService<SysUserService>().AddUser(new AddUserInput()
            {
                Account = name,
                RealName = name,
                NickName = name,
                Email = email,
                Avatar = avatarUrl,
                Phone = mobilePhone,
                OrgId = 1300000000101, // 根组织架构
                RoleIdList = new List<long> { 1300000000104 } // 仅本人数据角色
            });

            await _sysWechatUserRep.InsertAsync(new SysWechatUser()
            {
                UserId = userId,
                OpenId = openIdClaim.Value,
                Avatar = avatarUrl,
                NickName = name,
                PlatformType = platformType
            });

            wechatUser = await _sysWechatUserRep.AsQueryable().Includes(u => u.SysUser).ClearFilter().FirstAsync(u => u.OpenId == openIdClaim.Value);
        }

        // 构建Token令牌
        var token = await App.GetRequiredService<SysAuthService>().CreateToken(wechatUser.SysUser);

        return new RedirectResult($"{redirectUrl}/#/login?token={token.AccessToken}");
    }
}