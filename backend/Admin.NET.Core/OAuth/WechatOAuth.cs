using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.OAuth
{
    public class WechatOAuth : IWechatOAuth, ISingleton
    {
        private readonly string _authorizeUrl = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private readonly string _accessTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private readonly string _refreshTokenUrl = "https://api.weixin.qq.com/sns/oauth2/refresh_token";
        private readonly string _userInfoUrl = "https://api.weixin.qq.com/sns/userinfo";

        private readonly OAuthConfig _oauthConfig;

        public WechatOAuth(IConfiguration configuration)
        {
            _oauthConfig = OAuthConfig.LoadFrom(configuration, "oauth:wechat");
        }

        /// <summary>
        /// 发起授权
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string state = "")
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = _oauthConfig.AppId,
                ["redirect_uri"] = _oauthConfig.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = _oauthConfig.Scope,
                ["state"] = state
            };
            return $"{_authorizeUrl}?{param.ToQueryString()}#wechat_redirect";
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<TokenModel> GetAccessTokenAsync(string code, string state = "")
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = _oauthConfig.AppId,
                ["secret"] = _oauthConfig.AppKey,
                ["code"] = code,
                ["grant_type"] = "authorization_code"
            };
            var accessTokenModel = await $"{_accessTokenUrl}?{param.ToQueryString()}".GetAsAsync<TokenModel>();
            if (accessTokenModel.HasError())
                throw Oops.Oh($"{ accessTokenModel.ErrorDescription}");
            return accessTokenModel;
        }

        /// <summary>
        /// 获取微信用户基本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfoModel> GetUserInfoAsync(string accessToken, string openId)
        {
            var param = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
                ["openid"] = openId,
                ["lang"] = "zh_CN",
            };
            var userInfoModel = await $"{_userInfoUrl}?{param.ToQueryString()}".GetAsAsync<UserInfoModel>();
            if (userInfoModel.HasError())
                throw Oops.Oh($"{ userInfoModel.ErrorMessage}");
            return userInfoModel;
        }

        /// <summary>
        /// 刷新微信Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<TokenModel> GetRefreshTokenAsync(string refreshToken)
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = _oauthConfig.AppId,
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };
            var refreshTokenModel = await $"{_refreshTokenUrl}?{param.ToQueryString()}".GetAsAsync<TokenModel>();
            if (refreshTokenModel.HasError())
                throw Oops.Oh($"{ refreshTokenModel.ErrorDescription}");
            return refreshTokenModel;
        }
    }
}