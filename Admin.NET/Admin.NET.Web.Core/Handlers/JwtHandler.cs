using Admin.NET.Core;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext(),
                App.GetOptions<JWTSettingsOptions>().ExpiredTime,
                App.GetOptions<RefreshTokenOptions>().ExpiredTime))
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
        /// 检查权限
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private static async Task<bool> CheckAuthorzieAsync(DefaultHttpContext httpContext)
        {
            //// 管理员跳过判断
            //var userManager = App.GetService<UserManager>();
            //if (userManager.SuperAdmin) return true;

            //// 路由名称
            //var routeName = httpContext.Request.Path.Value[1..].Replace("/", ":");

            return await Task.FromResult(true);
        }
    }
}