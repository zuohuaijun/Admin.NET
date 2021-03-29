using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UAParser;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 登录授权相关服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Auth", Order = 160)]
    public class AuthService : IAuthService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysUser> _sysUserRep;     // 用户表仓储  
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager _userManager; // 用户管理

        private readonly ISysUserService _sysUserService; // 系统用户服务  
        private readonly ISysEmpService _sysEmpService;   // 系统员工服务      
        private readonly ISysRoleService _sysRoleService; // 系统角色服务  
        private readonly ISysMenuService _sysMenuService; // 系统菜单服务
        private readonly ISysAppService _sysAppService;   // 系统应用服务
        private readonly IClickWordCaptcha _captchaHandle;// 验证码服务
        private readonly ISysConfigService _sysConfigService; // 验证码服务

        public AuthService(IRepository<SysUser> sysUserRep,
                           IHttpContextAccessor httpContextAccessor,
                           IUserManager userManager,
                           ISysUserService sysUserService,
                           ISysEmpService sysEmpService,
                           ISysRoleService sysRoleService,
                           ISysMenuService sysMenuService,
                           ISysAppService sysAppService,
                           IClickWordCaptcha captchaHandle,
                           ISysConfigService sysConfigService)
        {
            _sysUserRep = sysUserRep;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _sysUserService = sysUserService;
            _sysEmpService = sysEmpService;
            _sysRoleService = sysRoleService;
            _sysMenuService = sysMenuService;
            _sysAppService = sysAppService;
            _captchaHandle = captchaHandle;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>默认用户名/密码：admin/admin</remarks>
        /// <returns></returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<string> LoginAsync([Required] LoginInput input)
        {
            // 获取加密后的密码
            var encryptPasswod = MD5Encryption.Encrypt(input.Password);

            // 判断用户名和密码是否正确
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Account.Equals(input.Account) && u.Password.Equals(encryptPasswod));
            _ = user ?? throw Oops.Oh(ErrorCode.D1000);

            // 验证账号是否被冻结
            if (user.Status == CommonStatus.DISABLE)
                throw Oops.Oh(ErrorCode.D1017);

            // 生成Token令牌
            //var accessToken = await _jwtBearerManager.CreateTokenAdmin(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimConst.CLAINM_USERID, user.Id },
                { ClaimConst.CLAINM_ACCOUNT, user.Account },
                { ClaimConst.CLAINM_NAME, user.Name },
                { ClaimConst.CLAINM_SUPERADMIN, user.AdminType },
            });

            // 设置Swagger自动登录
            _httpContextAccessor.SigninToSwagger(accessToken);

            // 生成刷新Token令牌
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 30);

            // 设置刷新Token令牌
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return accessToken;
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getLoginUser")]
        public async Task<LoginOutput> GetLoginUserAsync()
        {
            var user = _userManager.User;
            var userId = user.Id;

            var httpContext = App.GetService<IHttpContextAccessor>().HttpContext;
            var loginOutput = user.Adapt<LoginOutput>();

            loginOutput.LastLoginTime = user.LastLoginTime = DateTimeOffset.Now;
            loginOutput.LastLoginIp = user.LastLoginIp = httpContext.GetRemoteIpAddressToIPv4();

            //var ipInfo = IpTool.Search(loginOutput.LastLoginIp);
            //loginOutput.LastLoginAddress = ipInfo.Country + ipInfo.Province + ipInfo.City + "[" + ipInfo.NetworkOperator + "][" + ipInfo.Latitude + ipInfo.Longitude + "]";

            var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            loginOutput.LastLoginBrowser = clent.UA.Family + clent.UA.Major;
            loginOutput.LastLoginOs = clent.OS.Family + clent.OS.Major;

            // 员工信息
            loginOutput.LoginEmpInfo = await _sysEmpService.GetEmpInfo(userId);

            // 角色信息
            loginOutput.Roles = await _sysRoleService.GetUserRoleList(userId);

            // 权限信息
            loginOutput.Permissions = await _sysMenuService.GetLoginPermissionList(userId);

            // 数据范围信息(机构Id集合)
            loginOutput.DataScopes = await _sysUserService.GetUserDataScopeIdList(userId);

            // 具备应用信息（多系统，默认激活一个，可根据系统切换菜单）,返回的结果中第一个为激活的系统
            loginOutput.Apps = await _sysAppService.GetLoginApps(userId);

            // 菜单信息
            if (loginOutput.Apps.Count > 0)
            {
                var defaultActiveAppCode = loginOutput.Apps.FirstOrDefault(u => u.Active == YesOrNot.Y.ToString()).Code; // loginOutput.Apps[0].Code;
                loginOutput.Menus = await _sysMenuService.GetLoginMenusAntDesign(userId, defaultActiveAppCode);
            }

            // 增加登录日志
            await new SysLogVis
            {
                Name = "登录",
                Success = YesOrNot.Y.ToString(),
                Message = "登录成功",
                Ip = loginOutput.LastLoginIp,
                Browser = loginOutput.LastLoginBrowser,
                Os = loginOutput.LastLoginOs,
                VisType = 1,
                VisTime = loginOutput.LastLoginTime,
                Account = loginOutput.Account
            }.InsertAsync();

            return loginOutput;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpGet("/logout")]
        public async Task LogoutAsync()
        {
            _httpContextAccessor.SignoutToSwagger();
            //_httpContextAccessor.HttpContext.Response.Headers["access-token"] = "invalid token";

            // 增加退出日志
            await new SysLogVis
            {
                Name = "退出",
                Success = YesOrNot.Y.ToString(),
                Message = "退出成功",
                VisType = 2
            }.InsertAsync();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取验证码开关
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getCaptchaOpen")]
        [AllowAnonymous]
        public async Task<dynamic> GetCaptchaOpen()
        {
            return await _sysConfigService.GetCaptchaOpenFlag();
        }

        /// <summary>
        /// 获取验证码（默认点选模式）
        /// </summary>
        /// <returns></returns>
        [HttpPost("/captcha/get")]
        [AllowAnonymous]
        [NonUnify]
        public async Task<dynamic> GetCaptcha()
        {
            // 图片大小要与前端保持一致（坐标范围）
            return await Task.FromResult(_captchaHandle.CreateCaptchaImage(_captchaHandle.RandomCode(6), 310, 155));
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/captcha/check")]
        [AllowAnonymous]
        [NonUnify]
        public async Task<dynamic> VerificationCode(ClickWordCaptchaInput input)
        {
            return await Task.FromResult(_captchaHandle.CheckCode(input));
        }
    }
}
