using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient;
using SKIT.FlurlHttpClient.Wechat.Api;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 微信API客户端
    /// </summary>
    [ApiDescriptionSettings(false)]
    public partial class WeChatApiHttpClient : ISingleton
    {
        public readonly WechatOptions _weChatOptions;

        public WeChatApiHttpClient(IOptions<WechatOptions> weChatOptions)
        {
            _weChatOptions = weChatOptions.Value;
        }

        /// <summary>
        /// 微信公众号
        /// </summary>
        /// <returns></returns>
        public WechatApiClient CreateWechatClient()
        {
            if (string.IsNullOrEmpty(_weChatOptions.WechatAppId) || string.IsNullOrEmpty(_weChatOptions.WechatAppSecret))
                throw Oops.Oh("微信公众号配置错误");

            var wechatApiClient = new WechatApiClient(new WechatApiClientOptions()
            {
                AppId = _weChatOptions.WechatAppId,
                AppSecret = _weChatOptions.WechatAppSecret,
            });

            wechatApiClient.Configure(settings =>
            {
                settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
            });

            return wechatApiClient;
        }

        /// <summary>
        /// 微信小程序
        /// </summary>
        /// <returns></returns>
        public WechatApiClient CreateWxOpenClient()
        {
            if (string.IsNullOrEmpty(_weChatOptions.WxOpenAppId) || string.IsNullOrEmpty(_weChatOptions.WxOpenAppSecret))
                throw Oops.Oh("微信小程序配置错误");

            var WechatApiClient = new WechatApiClient(new WechatApiClientOptions()
            {
                AppId = _weChatOptions.WxOpenAppId,
                AppSecret = _weChatOptions.WxOpenAppSecret
            });

            WechatApiClient.Configure(settings =>
            {
                settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
            });

            return WechatApiClient;
        }
    }
}