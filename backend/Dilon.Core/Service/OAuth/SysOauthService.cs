using Dilon.Core.OAuth;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service.OAuth
{
    /// <summary>
    /// OAuth服务
    /// </summary>
    [ApiDescriptionSettings(Name = "OAuth", Order = 159)]
    [AllowAnonymous]
    public class SysOauthService : ISysOauthService, IDynamicApiController, ITransient
    {
        private readonly HttpContext _httpContext;
        private readonly WechatOAuth _wechatOAuth;

        public SysOauthService(IHttpContextAccessor httpContextAccessor, WechatOAuth wechatOAuth)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _wechatOAuth = wechatOAuth;
        }

        /// <summary>
        /// 微信登录授权
        /// </summary>
        [HttpGet("oauth/wechat")]
        public Task WechatLogin()
        {
            _httpContext.Response.Redirect(_wechatOAuth.GetAuthorizeUrl("Dilon"));
            return Task.CompletedTask;
        }

        /// <summary>
        /// 微信登录授权回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="error_description"></param>
        /// <returns></returns>
        [HttpGet("oauth/wechatcallback")]
        public async Task WechatLoginCallback([FromQuery] string code, [FromQuery] string state, [FromQuery] string error_description = "")
        {
            if (!string.IsNullOrEmpty(error_description))
                throw Oops.Oh(error_description);

            var accessTokenModel = await _wechatOAuth.GetAccessTokenAsync(code, state);
            //var userInfoModel = await _wechatOAuth.GetUserInfoAsync(accessTokenModel.AccessToken, accessTokenModel.OpenId);
            await _httpContext.Response.WriteAsJsonAsync(accessTokenModel);
        }

        /// <summary>
        /// 获取微信用户基本信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet("oauth/wechat/user")]
        public async Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string openId)
        {
            return await _wechatOAuth.GetUserInfoAsync(token, openId);
        }
    }
}