using Admin.NET.Core;
using Admin.NET.Core.Service;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Admin.NET.Web.Core
{
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 自动刷新Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 获取Token过期时间
            var serviceProvider = context.GetCurrentHttpContext().RequestServices;
            var sysConfigService = serviceProvider.GetService<SysConfigService>();
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
                currentHttpContext.SignoutToSwagger();
            }
        }

        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 已自动验证 Jwt Token 有效性
            return await CheckAuthorzieAsync(httpContext);
        }

        /// <summary>
        /// 权限校验核心逻辑
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private static async Task<bool> CheckAuthorzieAsync(DefaultHttpContext httpContext)
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