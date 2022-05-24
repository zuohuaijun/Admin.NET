using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 微信公众号服务
    /// </summary>
    [ApiDescriptionSettings(Name = "微信公众号", Order = 100)]
    public class WechatService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<WechatUser> _wechatUserRep;
        private readonly WechatApiClient _wechatApiClient;

        public WechatService(SqlSugarRepository<WechatUser> wechatUserRep,
            WeChatApiHttpClient wechatApiHttpClient)
        {
            _wechatUserRep = wechatUserRep;
            _wechatApiClient = wechatApiHttpClient.CreateWechatClient();
        }

        /// <summary>
        /// 生成网页授权Url
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/weChat/genAuthUrl")]
        [AllowAnonymous]
        public string GenAuthUrl(GenAuthUrlInput input)
        {
            return _wechatApiClient.GenerateParameterizedUrlForConnectOAuth2Authorize(input.RedirectUrl, input.Scope);
        }

        /// <summary>
        /// 授权登录(Code换取OpenId)
        /// </summary>
        /// <param name="input"></param>
        [HttpPost("/weChat/snsOAuth2")]
        [AllowAnonymous]
        public async Task<string> LoginOAuth2([Required] WechatOAuth2Input input)
        {
            var reqOAuth2 = new SnsOAuth2AccessTokenRequest()
            {
                Code = input.Code,
            };
            var resOAuth2 = await _wechatApiClient.ExecuteSnsOAuth2AccessTokenAsync(reqOAuth2);
            if (resOAuth2.ErrorCode != (int)WeChatReturnCodeEnum.请求成功)
                throw Oops.Oh(resOAuth2.ErrorMessage + resOAuth2.ErrorCode);

            var wxUser = await _wechatUserRep.GetFirstAsync(p => p.OpenId == resOAuth2.OpenId);
            if (wxUser == null)
            {
                var reqUserInfo = new SnsUserInfoRequest()
                {
                    OpenId = resOAuth2.OpenId,
                    AccessToken = resOAuth2.AccessToken,
                };
                var resUserInfo = await _wechatApiClient.ExecuteSnsUserInfoAsync(reqUserInfo);
                wxUser = resUserInfo.Adapt<WechatUser>();
                wxUser.Avatar = resUserInfo.HeadImageUrl;
                wxUser.NickName = resUserInfo.Nickname;
                wxUser = await _wechatUserRep.AsInsertable(wxUser).ExecuteReturnEntityAsync();
            }
            else
            {
                wxUser.AccessToken = resOAuth2.AccessToken;
                wxUser.RefreshToken = resOAuth2.RefreshToken;
                await _wechatUserRep.AsUpdateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
            }

            return resOAuth2.OpenId;
        }

        /// <summary>
        /// 微信用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/weChat/openIdLogin")]
        [AllowAnonymous]
        public async Task<dynamic> WechatUserLogin(WechatUserLogin input)
        {
            var wxUser = await _wechatUserRep.GetFirstAsync(p => p.OpenId == input.OpenId);
            if (wxUser == null)
                throw Oops.Oh("微信用户不存在");
            return new
            {
                wxUser.Id,
                wxUser.OpenId,
                wxUser.NickName,
                wxUser.Avatar,
                accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
                {
                    { ClaimConst.UserId, wxUser.Id },
                    { ClaimConst.OpenId, wxUser.OpenId },
                    { ClaimConst.RealName, wxUser.NickName },
                })
            };
        }

        /// <summary>
        /// 获取配置签名参数(wx.config)
        /// </summary>
        /// <returns></returns>
        [HttpPost("/weChat/genConfigPara")]
        public async Task<dynamic> GenConfigPara(SignatureInput input)
        {
            var resCgibinToken = await _wechatApiClient.ExecuteCgibinTokenAsync(new CgibinTokenRequest());
            var request = new CgibinTicketGetTicketRequest()
            {
                AccessToken = resCgibinToken.AccessToken
            };
            var response = await _wechatApiClient.ExecuteCgibinTicketGetTicketAsync(request);
            if (!response.IsSuccessful())
                throw Oops.Oh(response.ErrorMessage);
            return _wechatApiClient.GenerateParametersForJSSDKConfig(response.Ticket, input.Url);
        }
    }
}