// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Admin.NET.Web.Core
{
    public class JwtHandler : AppAuthorizeHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public JwtHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 自动刷新Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // var serviceProvider = context.GetCurrentHttpContext().RequestServices;
            using var serviceScope = _serviceProvider.CreateScope();

            // 若当前账号存在黑名单中则授权失败
            var sysCacheService = serviceScope.ServiceProvider.GetService<SysCacheService>();
            if (sysCacheService.ExistKey($"{CacheConst.KeyBlacklist}{context.User.FindFirst(ClaimConst.UserId)?.Value}"))
            {
                context.Fail();
                context.GetCurrentHttpContext().SignoutToSwagger();
                return;
            }

            var sysConfigService = serviceScope.ServiceProvider.GetService<SysConfigService>();
            var tokenExpire = await sysConfigService.GetTokenExpire();
            var refreshTokenExpire = await sysConfigService.GetRefreshTokenExpire();
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext(), tokenExpire, refreshTokenExpire))
            {
                await AuthorizeHandleAsync(context);
            }
            else
            {
                context.Fail(); // 授权失败
                var currentHttpContext = context.GetCurrentHttpContext();
                if (currentHttpContext == null)
                    return;
                // 跳过由于 SignatureAuthentication 引发的失败
                if (currentHttpContext.Items.ContainsKey(SignatureAuthenticationDefaults.AuthenticateFailMsgKey))
                    return;
                currentHttpContext.SignoutToSwagger();
            }
        }

        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 已自动验证 Jwt Token 有效性
            return await CheckAuthorizeAsync(httpContext);
        }

        /// <summary>
        /// 权限校验核心逻辑
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private static async Task<bool> CheckAuthorizeAsync(DefaultHttpContext httpContext)
        {
            // 登录模式判断PC、APP
            if (App.User.FindFirst(ClaimConst.LoginMode)?.Value == ((int)LoginModeEnum.APP).ToString())
                return true;

            // 排除超管
            if (App.User.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString())
                return true;

            // 路由名称
            var routeName = httpContext.Request.Path.StartsWithSegments("/api")
                ? httpContext.Request.Path.Value[5..].Replace("/", ":")
                : httpContext.Request.Path.Value[1..].Replace("/", ":");

            // 获取用户拥有按钮权限集合
            var ownBtnPermList = await App.GetService<SysMenuService>().GetOwnBtnPermList();
            // 获取系统所有按钮权限集合
            var allBtnPermList = await App.GetService<SysMenuService>().GetAllBtnPermList();

            // 已拥有该按钮权限或者所有按钮集合里面不存在
            var exist1 = ownBtnPermList.Exists(u => routeName.Equals(u, StringComparison.CurrentCultureIgnoreCase));
            var exist2 = allBtnPermList.TrueForAll(u => !routeName.Equals(u, StringComparison.CurrentCultureIgnoreCase));
            return exist1 || exist2;
        }
    }
}