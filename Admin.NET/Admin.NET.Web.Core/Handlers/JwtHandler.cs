// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
            // 获取Token过期时间
            // var serviceProvider = context.GetCurrentHttpContext().RequestServices;
            using var serviceScope = _serviceProvider.CreateScope();
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
                DefaultHttpContext currentHttpContext = context.GetCurrentHttpContext();
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
            var exist1 = ownBtnPermList.Exists(u => routeName.Equals(u, System.StringComparison.CurrentCultureIgnoreCase));
            var exist2 = allBtnPermList.TrueForAll(u => !routeName.Equals(u, System.StringComparison.CurrentCultureIgnoreCase));
            return exist1 || exist2;
        }
    }
}