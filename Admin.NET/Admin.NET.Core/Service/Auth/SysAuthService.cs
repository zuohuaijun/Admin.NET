using Furion.SpecificationDocument;
using Microsoft.Extensions.Caching.Memory;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
[ApiDescriptionSettings(Order = 200)]
public class SysAuthService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserManager _userManager;
    private readonly IEventPublisher _eventPublisher;
    private readonly SysUserService _sysUserService;
    private readonly SysUserRoleService _sysUserRoleService;
    private readonly ISysOnlineUserService _sysOnlineUserService;
    private readonly IMemoryCache _cache;

    public SysAuthService(SqlSugarRepository<SysUser> sysUserRep,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        IHttpContextAccessor httpContextAccessor,
        IUserManager userManager,
        IEventPublisher eventPublisher,
        SysUserService sysUserService,
        SysUserRoleService sysUserRoleService,
        ISysOnlineUserService sysOnlineUserService,
        IMemoryCache cache)
    {
        _sysUserRep = sysUserRep;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _eventPublisher = eventPublisher;
        _sysUserService = sysUserService;
        _sysUserRoleService = sysUserRoleService;
        _sysOnlineUserService = sysOnlineUserService;
        _cache = cache;
    }

    /// <summary>
    /// 登录系统
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：vben/123456</remarks>
    /// <returns></returns>
    [HttpPost("/login")]
    [AllowAnonymous]
    [SuppressMonitor]
    public async Task<LoginOutput> Login([Required] LoginInput input)
    {
        var encryptPasswod = MD5Encryption.Encrypt(input.Password); // 加密密码

        // 判断用户名密码
        var user = await _sysUserRep.AsQueryable().Includes(u => u.SysOrg)
            .FirstAsync(u => u.UserName.Equals(input.UserName) && u.Password.Equals(encryptPasswod));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D1000);

        // 验证账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.D1017);

        // 单用户登录（强制下线其他地方登录账号）
        await _sysOnlineUserService.SignleLogin(user.Id);

        // 生成Token令牌
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            {ClaimConst.UserId, user.Id},
            {ClaimConst.TenantId, user.TenantId},
            {ClaimConst.UserName, user.UserName},
            {ClaimConst.RealName, user.RealName},
            {ClaimConst.SuperAdmin, user.UserType},
            {ClaimConst.OrgId, user.OrgId},
            {ClaimConst.OrgName, user.SysOrg?.Name},
            {ClaimConst.OrgLevel, user.SysOrg?.Level},
        });

        // 生成刷新Token令牌
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, _refreshTokenOptions.ExpiredTime);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.Response.Headers["access-token"] = accessToken;
        _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

        // Swagger Knife4UI-AfterScript登录脚本
        // ke.global.setAllHeader('Authorization', 'Bearer ' + ke.response.headers['access-token']);

        return new LoginOutput
        {
            UserId = user.Id,
            Token = accessToken
        };
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/getUserInfo")]
    public async Task<LoginUserInfoOutput> GetUserInfo()
    {
        var user = _userManager.User;
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1011);

        // 角色信息
        var roles = await _sysUserRoleService.GetUserRoleList(user.Id);

        // 数据范围
        var dataScopes = await _sysUserService.GetUserOrgIdList();

        // 登录日志
        var client = Parser.GetDefault().Parse(_httpContextAccessor.HttpContext.Request.Headers["User-Agent"]);
        await _eventPublisher.PublishAsync("Add:VisLog", new SysLogVis
        {
            Success = YesNoEnum.Y,
            Message = "登录",
            Ip = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4(),
            Browser = client.UA.Family + client.UA.Major,
            Os = client.OS.Family + client.OS.Major,
            VisType = LoginTypeEnum.Login,
            UserName = user.UserName,
            RealName = user.RealName
        });

        return new LoginUserInfoOutput
        {
            UserId = user.Id,
            Username = user.UserName,
            RealName = user.RealName,
            Avatar = user.Avatar,
            Desc = user.Introduction,
            OrgId = user.OrgId,
            OrgName = user.SysOrg != null ? user.SysOrg.Name : "",
            OrgLevel = user.SysOrg != null ? user.SysOrg.Level : "",
            Roles = roles.Select(u => new LoginRole
            {
                RoleName = u.Name,
                Value = u.Code
            }).ToList(),
        };
    }

    /// <summary>
    /// 获取刷新Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    [HttpPost("/getRefreshToken")]
    public string RefreshToken([Required] string accessToken)
    {
        return JWTEncryption.GenerateRefreshToken(accessToken, _refreshTokenOptions.ExpiredTime);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    [HttpGet("/logout")]
    public async void Logout()
    {
        var user = _userManager.User;
        if (user == null)
            throw Oops.Oh(ErrorCodeEnum.D1011);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.Response.Headers["access-token"] = "invalid_token";
        _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = "invalid_token";

        // 退出日志
        await _eventPublisher.PublishAsync("Add:VisLog", new SysLogVis
        {
            Success = YesNoEnum.Y,
            Message = "退出",
            VisType = LoginTypeEnum.Logout,
            Ip = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4(),
            UserName = user.UserName,
            RealName = user.RealName
        });
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