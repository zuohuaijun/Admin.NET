using Furion.SpecificationDocument;
using Lazy.Captcha.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
[ApiDescriptionSettings(Order = 500)]
public class SysAuthService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SysMenuService _sysMenuService;
    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly IMemoryCache _cache;
    private readonly ICaptcha _captcha;

    public SysAuthService(UserManager userManager,
        SqlSugarRepository<SysUser> sysUserRep,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IHttpContextAccessor httpContextAccessor,
        SysMenuService sysMenuService,
        SysOnlineUserService sysOnlineUserService,
        SysConfigService sysConfigService,
        IMemoryCache cache,
        ICaptcha captcha)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _sysMenuService = sysMenuService;
        _sysOnlineUserService = sysOnlineUserService;
        _sysConfigService = sysConfigService;
        _cache = cache;
        _captcha = captcha;
    }

    /// <summary>
    /// 登录系统
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "Login")]
    [DisplayName("登录系统")]
    public async Task<LoginOutput> Login([Required] LoginInput input)
    {
        //// 可以根据域名获取具体租户
        //var host = _httpContextAccessor.HttpContext.Request.Host;

        // 是否开启验证码
        if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha))
        {
            // 判断验证码
            if (!_captcha.Validate(input.CodeId.ToString(), input.Code))
                throw Oops.Oh(ErrorCodeEnum.D0008);
        }

        // 账号是否存在
        var user = await _sysUserRep.AsQueryable().Filter(null, true).FirstAsync(u => u.Account.Equals(input.Account));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.D1017);

        // 租户是否被禁用
        var tenant = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysTenant>>().GetFirstAsync(u => u.Id == user.TenantId);
        if (tenant.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.Z1003);

        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (user.Password != MD5Encryption.Encrypt(input.Password))
                throw Oops.Oh(ErrorCodeEnum.D1000);
        }
        else
        {
            if (CryptogramUtil.Decrypt(user.Password) != input.Password)
                throw Oops.Oh(ErrorCodeEnum.D1000);
        }

        // 单用户登录
        await _sysOnlineUserService.SignleLogin(user.Id);

        // 生成Token令牌
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            {ClaimConst.UserId, user.Id},
            {ClaimConst.TenantId, user.TenantId},
            {ClaimConst.Account, user.Account},
            {ClaimConst.RealName, user.RealName},
            {ClaimConst.AccountType, user.AccountType },
            {ClaimConst.OrgId, user.OrgId},
        });

        // 生成刷新Token令牌
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, _refreshTokenOptions.ExpiredTime);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(accessToken, refreshToken);

        // Swagger Knife4UI-AfterScript登录脚本
        // ke.global.setAllHeader('Authorization', 'Bearer ' + ke.response.headers['access-token']);

        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// 获取登录账号
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "UserInfo")]
    [DisplayName("登录系统")]
    public async Task<LoginUserOutput> GetUserInfo()
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1011);

        var org = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysOrg>>().GetFirstAsync(u => u.Id == user.OrgId);
        var pos = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysPos>>().GetFirstAsync(u => u.Id == user.PosId);

        // 按钮权限集合
        var buttons = await _sysMenuService.GetBtnPermissionList();

        return new LoginUserOutput
        {
            Account = user.Account,
            RealName = user.RealName,
            Avatar = user.Avatar,
            Address = user.Address,
            Signature = user.Signature,
            OrgId = user.OrgId,
            OrgName = org?.Name,
            PosName = pos?.Name,
            Buttons = buttons
        };
    }

    /// <summary>
    /// 获取刷新Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "RefreshToken")]
    [DisplayName("获取刷新Token")]
    public string GetRefreshToken([Required] string accessToken)
    {
        return JWTEncryption.GenerateRefreshToken(accessToken, _refreshTokenOptions.ExpiredTime);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    [ApiDescriptionSettings(Name = "Logout")]
    [DisplayName("退出系统")]
    public void Logout()
    {
        if (string.IsNullOrWhiteSpace(_userManager.Account))
            throw Oops.Oh(ErrorCodeEnum.D1011);

        // 置空响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(null, null);
    }

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [ApiDescriptionSettings(Name = "LoginConfig")]
    [DisplayName("获取登录配置")]
    public async Task<dynamic> GetLoginConfig()
    {
        var secondVerEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSecondVer);
        var captchaEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
        var wartermarkEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysWartermark);
        return new { SecondVerEnabled = secondVerEnabled, CaptchaEnabled = captchaEnabled, WartermarkEnabled = wartermarkEnabled };
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [ApiDescriptionSettings(Name = "Captcha")]
    [DisplayName("获取验证码")]
    public dynamic GetCaptcha()
    {
        var codeId = YitIdHelper.NextId();
        var captcha = _captcha.Generate(codeId.ToString());
        return new { Id = codeId, Img = captcha.Base64 };
    }

    /// <summary>
    /// swagger登录检查
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/api/swagger/checkUrl"), NonUnify]
    [DisplayName("swagger登录检查")]
    public int SwaggerCheckUrl()
    {
        return _cache.Get<bool>(CacheConst.SwaggerLogin) ? 200 : 401;
    }

    /// <summary>
    /// swagger登录提交
    /// </summary>
    /// <param name="auth"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/api/swagger/submitUrl"), NonUnify]
    [DisplayName("swagger登录提交")]
    public int SwaggerSubmitUrl([FromForm] SpecificationAuth auth)
    {
        var userName = App.GetConfig<string>("SpecificationDocumentSettings:LoginInfo:UserName");
        var password = App.GetConfig<string>("SpecificationDocumentSettings:LoginInfo:Password");
        if (auth.UserName == userName && auth.Password == password)
        {
            _cache.Set<bool>(CacheConst.SwaggerLogin, true);
            return 200;
        }
        return 401;
    }
}