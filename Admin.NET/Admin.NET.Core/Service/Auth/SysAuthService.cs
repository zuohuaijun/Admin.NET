// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Furion.SpecificationDocument;
using Lazy.Captcha.Core;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统登录授权服务 💥
/// </summary>
[ApiDescriptionSettings(Order = 500)]
public class SysAuthService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SysMenuService _sysMenuService;
    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly ICaptcha _captcha;
    private readonly SysCacheService _sysCacheService;

    public SysAuthService(UserManager userManager,
        SqlSugarRepository<SysUser> sysUserRep,
        IHttpContextAccessor httpContextAccessor,
        SysMenuService sysMenuService,
        SysOnlineUserService sysOnlineUserService,
        SysConfigService sysConfigService,
        ICaptcha captcha,
        SysCacheService sysCacheService)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _httpContextAccessor = httpContextAccessor;
        _sysMenuService = sysMenuService;
        _sysOnlineUserService = sysOnlineUserService;
        _sysConfigService = sysConfigService;
        _captcha = captcha;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 账号密码登录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("账号密码登录")]
    public virtual async Task<LoginOutput> Login([Required] LoginInput input)
    {
        //// 可以根据域名获取具体租户
        //var host = _httpContextAccessor.HttpContext.Request.Host;

        // 判断密码错误次数（默认5次，缓存30分钟）
        var keyErrorPasswordCount = $"{CacheConst.KeyErrorPasswordCount}{input.Account}";
        var errorPasswordCount = _sysCacheService.Get<int>(keyErrorPasswordCount);
        if (errorPasswordCount >= 5)
            throw Oops.Oh(ErrorCodeEnum.D1027);

        // 是否开启验证码
        if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha))
        {
            // 判断验证码
            if (!_captcha.Validate(input.CodeId.ToString(), input.Code))
                throw Oops.Oh(ErrorCodeEnum.D0008);
        }

        // 账号是否存在
        var user = await _sysUserRep.AsQueryable().Includes(t => t.SysOrg).ClearFilter().FirstAsync(u => u.Account.Equals(input.Account));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        // 账号是否被冻结
        if (user.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.D1017);

        // 租户是否被禁用
        var tenant = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysTenant>>().GetFirstAsync(u => u.Id == user.TenantId);
        if (tenant != null && tenant.Status == StatusEnum.Disable)
            throw Oops.Oh(ErrorCodeEnum.Z1003);

        // 国密SM2解密（前端密码传输SM2加密后的）
        input.Password = CryptogramUtil.SM2Decrypt(input.Password);

        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (!user.Password.Equals(MD5Encryption.Encrypt(input.Password)))
            {
                _sysCacheService.Set(keyErrorPasswordCount, ++errorPasswordCount, TimeSpan.FromMinutes(30));
                throw Oops.Oh(ErrorCodeEnum.D1000);
            }
        }
        else
        {
            if (!CryptogramUtil.Decrypt(user.Password).Equals(input.Password))
            {
                _sysCacheService.Set(keyErrorPasswordCount, ++errorPasswordCount, TimeSpan.FromMinutes(30));
                throw Oops.Oh(ErrorCodeEnum.D1000);
            }
        }

        // 登录成功则清空密码错误次数
        _sysCacheService.Remove(keyErrorPasswordCount);

        return await CreateToken(user);
    }

    /// <summary>
    /// 验证锁屏密码 🔖
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    [DisplayName("验证锁屏密码")]
    public virtual async Task<bool> UnLockScreen([Required, FromQuery] string password)
    {
        // 账号是否存在
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        // 国密SM2解密（前端密码传输SM2加密后的）
        password = CryptogramUtil.SM2Decrypt(password);

        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (!user.Password.Equals(MD5Encryption.Encrypt(password)))
                throw Oops.Oh(ErrorCodeEnum.D1000);
        }
        else
        {
            if (!CryptogramUtil.Decrypt(user.Password).Equals(password))
                throw Oops.Oh(ErrorCodeEnum.D1000);
        }

        return true;
    }

    /// <summary>
    /// 手机号登录 🔖
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("手机号登录")]
    public virtual async Task<LoginOutput> LoginPhone([Required] LoginPhoneInput input)
    {
        var verifyCode = _sysCacheService.Get<string>($"{CacheConst.KeyPhoneVerCode}{input.Phone}");
        if (string.IsNullOrWhiteSpace(verifyCode))
            throw Oops.Oh("验证码不存在或已失效，请重新获取！");
        if (verifyCode != input.Code)
            throw Oops.Oh("验证码错误！");

        // 账号是否存在
        var user = await _sysUserRep.AsQueryable().Includes(t => t.SysOrg).ClearFilter().FirstAsync(u => u.Phone.Equals(input.Phone));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        return await CreateToken(user);
    }

    /// <summary>
    /// 生成Token令牌 🔖
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [NonAction]
    public virtual async Task<LoginOutput> CreateToken(SysUser user)
    {
        // 单用户登录
        await _sysOnlineUserService.SingleLogin(user.Id);

        // 生成Token令牌
        var tokenExpire = await _sysConfigService.GetTokenExpire();
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimConst.UserId, user.Id },
            { ClaimConst.TenantId, user.TenantId },
            { ClaimConst.Account, user.Account },
            { ClaimConst.RealName, user.RealName },
            { ClaimConst.AccountType, user.AccountType },
            { ClaimConst.OrgId, user.OrgId },
            { ClaimConst.OrgName, user.SysOrg?.Name },
            { ClaimConst.OrgType, user.SysOrg?.Type },
        }, tokenExpire);

        // 生成刷新Token令牌
        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

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
    /// 获取登录账号 🔖
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取登录账号")]
    public virtual async Task<LoginUserOutput> GetUserInfo()
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId) ?? throw Oops.Oh(ErrorCodeEnum.D1011).StatusCode(401);
        // 获取机构
        var org = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysOrg>>().GetFirstAsync(u => u.Id == user.OrgId);
        // 获取职位
        var pos = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysPos>>().GetFirstAsync(u => u.Id == user.PosId);
        // 获取拥有按钮权限集合
        var buttons = await _sysMenuService.GetOwnBtnPermList();
        // 获取权限集合
        var roleIds = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysUserRole>>().AsQueryable()
            .Where(u => u.UserId == user.Id).Select(u => u.RoleId).ToListAsync();

        return new LoginUserOutput
        {
            Id = user.Id,
            Account = user.Account,
            RealName = user.RealName,
            Phone = user.Phone,
            IdCardNum = user.IdCardNum,
            Email = user.Email,
            AccountType = user.AccountType,
            Avatar = user.Avatar,
            Address = user.Address,
            Signature = user.Signature,
            OrgId = user.OrgId,
            OrgName = org?.Name,
            OrgType = org?.Type,
            PosName = pos?.Name,
            Buttons = buttons,
            RoleIds = roleIds
        };
    }

    /// <summary>
    /// 获取刷新Token 🔖
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    [DisplayName("获取刷新Token")]
    public virtual string GetRefreshToken([FromQuery] string accessToken)
    {
        var refreshTokenExpire = _sysConfigService.GetRefreshTokenExpire().GetAwaiter().GetResult();
        return JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);
    }

    /// <summary>
    /// 退出系统 🔖
    /// </summary>
    [DisplayName("退出系统")]
    public virtual void Logout()
    {
        if (string.IsNullOrWhiteSpace(_userManager.Account))
            throw Oops.Oh(ErrorCodeEnum.D1011);

        _httpContextAccessor.HttpContext.SignoutToSwagger();
    }

    /// <summary>
    /// 获取登录配置 🔖
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [DisplayName("获取登录配置")]
    public virtual async Task<dynamic> GetLoginConfig()
    {
        var secondVerEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSecondVer);
        var captchaEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
        return new { SecondVerEnabled = secondVerEnabled, CaptchaEnabled = captchaEnabled };
    }

    /// <summary>
    /// 获取水印配置 🔖
    /// </summary>
    /// <returns></returns>
    [SuppressMonitor]
    [DisplayName("获取水印配置")]
    public virtual async Task<dynamic> GetWatermarkConfig()
    {
        var watermarkEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysWatermark);
        return new { WatermarkEnabled = watermarkEnabled };
    }

    /// <summary>
    /// 获取验证码 🔖
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [DisplayName("获取验证码")]
    public virtual dynamic GetCaptcha()
    {
        var codeId = YitIdHelper.NextId().ToString();
        var captcha = _captcha.Generate(codeId);
        return new { Id = codeId, Img = captcha.Base64 };
    }

    /// <summary>
    /// Swagger登录检查 🔖
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/api/swagger/checkUrl"), NonUnify]
    [DisplayName("Swagger登录检查")]
    public virtual int SwaggerCheckUrl()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? 200 : 401;
    }

    /// <summary>
    /// Swagger登录提交 🔖
    /// </summary>
    /// <param name="auth"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("/api/swagger/submitUrl"), NonUnify]
    [DisplayName("Swagger登录提交")]
    public virtual async Task<int> SwaggerSubmitUrl([FromForm] SpecificationAuth auth)
    {
        try
        {
            _sysCacheService.Set(CommonConst.SysCaptcha, false);

            await Login(new LoginInput
            {
                Account = auth.UserName,
                Password = CryptogramUtil.SM2Encrypt(auth.Password),
            });

            _sysCacheService.Remove(CommonConst.SysCaptcha);

            return 200;
        }
        catch (Exception)
        {
            return 401;
        }
    }
}