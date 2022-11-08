using Furion.SpecificationDocument;
using Lazy.Captcha.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
[ApiDescriptionSettings(Order = 200)]
public class SysAuthService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEventPublisher _eventPublisher;
    private readonly SysMenuService _sysMenuService;
    private readonly SysTenantService _sysTenantService;
    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly IMemoryCache _cache;
    private readonly ICaptcha _captcha;

    public SysAuthService(UserManager userManager,
        SqlSugarRepository<SysUser> sysUserRep,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IHttpContextAccessor httpContextAccessor,
        IEventPublisher eventPublisher,
        SysMenuService sysMenuService,
        SysTenantService sysTenantService,
        SysOnlineUserService sysOnlineUserService,
        SysConfigService sysConfigService,
        IMemoryCache cache,
        ICaptcha captcha)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _eventPublisher = eventPublisher;
        _sysMenuService = sysMenuService;
        _sysTenantService = sysTenantService;
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
    [HttpPost("/login")]
    [AllowAnonymous]
    [SuppressMonitor]
    public async Task<LoginOutput> Login([Required] LoginInput input)
    {
        // 判断验证码
        var captchaEnabled = await GetCaptchaFlag();
        if (captchaEnabled && !_captcha.Validate(input.CodeId.ToString(), input.Code))
            throw Oops.Oh(ErrorCodeEnum.D0009);

        var encryptPasswod = MD5Encryption.Encrypt(input.Password);

        // 判断用户名密码
        Expression<Func<SysUser, bool>> sysUserExp = u => u.Account.Equals(input.Account) && u.Password.Equals(encryptPasswod);
        SysUser user = null;
        if (input.TenantId > 0)
        {
            var db = App.GetRequiredService<ISqlSugarClient>().AsTenant();
            user = await SqlSugarSetup.InitTenantDb(db, input.TenantId).Queryable<SysUser>().FirstAsync(sysUserExp);
        }
        else
        {
            user = await _sysUserRep.GetFirstAsync(sysUserExp);
        }
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D1000);

        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.D1017);

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
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/userInfo")]
    public async Task<LoginUserOutput> GetUserInfo()
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1011);

        var org = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysOrg>>().GetFirstAsync(u => u.Id == user.OrgId);
        var pos = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysPos>>().GetFirstAsync(u => u.Id == user.PosId);

        // 按钮权限集合
        var buttons = await _sysMenuService.GetPermCodeList();

        // 登录日志
        var ip = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4();
        var client = Parser.GetDefault().Parse(_httpContextAccessor.HttpContext.Request.Headers["User-Agent"]);
        //var ipInfo = IpTool.Search(ip);
        //var address = ipInfo.Country + ipInfo.Province + ipInfo.City + "[" + ipInfo.NetworkOperator + "][" + ipInfo.Latitude + ipInfo.Longitude + "]";
        await _eventPublisher.PublishAsync("Add:VisLog", new SysLogVis
        {
            Success = YesNoEnum.Y,
            Message = "登录",
            Ip = ip,
            //Location = address,
            Browser = client.UA.Family + client.UA.Major,
            Os = client.OS.Family + client.OS.Major,
            VisType = LoginTypeEnum.Login,
            Account = user.Account,
            RealName = user.RealName
        });

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
    [HttpPost("/refreshToken")]
    public string RefreshToken([Required] string accessToken)
    {
        return JWTEncryption.GenerateRefreshToken(accessToken, _refreshTokenOptions.ExpiredTime);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    [HttpPost("/logout")]
    public async void Logout()
    {
        if (string.IsNullOrWhiteSpace(_userManager.Account))
            throw Oops.Oh(ErrorCodeEnum.D1011);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(null, null);

        // 退出日志
        await _eventPublisher.PublishAsync("Add:VisLog", new SysLogVis
        {
            Success = YesNoEnum.Y,
            Message = "退出",
            VisType = LoginTypeEnum.Logout,
            Ip = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4(),
            Account = _userManager.Account,
            RealName = _userManager.RealName
        });
    }

    /// <summary>
    /// 是否启用验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet("/captchaFlag")]
    [AllowAnonymous]
    [SuppressMonitor]
    public async Task<bool> GetCaptchaFlag()
    {
        return await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
    }

    /// <summary>
    /// 生成图片验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet("/captcha")]
    [AllowAnonymous]
    [SuppressMonitor]
    public dynamic GetCaptcha()
    {
        var codeId = Yitter.IdGenerator.YitIdHelper.NextId();
        var captcha = _captcha.Generate(codeId.ToString());
        return new { Id = codeId, Img = captcha.Base64 };
    }

    /// <summary>
    /// 是否启用多库租户
    /// </summary>
    /// <returns></returns>
    [HttpGet("/tenantDbList")]
    [AllowAnonymous]
    [SuppressMonitor]
    public async Task<List<SysTenant>> GetTenantDbList()
    {
        var tenantDbEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysTenantDb);
        return tenantDbEnabled ? await _sysTenantService.GetTenantDbList() : new List<SysTenant>();
    }

    /// <summary>
    /// Swagger登录检查
    /// </summary>
    /// <returns></returns>
    [HttpPost("/Swagger/CheckUrl"), NonUnify]
    [AllowAnonymous]
    public int SwaggerCheckUrl()
    {
        return _cache.Get<bool>(CacheConst.SwaggerLogin) ? 200 : 401;
    }

    /// <summary>
    /// Swagger登录
    /// </summary>
    /// <param name="auth"></param>
    /// <returns></returns>
    [HttpPost("/Swagger/SubmitUrl"), NonUnify]
    [AllowAnonymous]
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