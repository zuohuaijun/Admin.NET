// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Admin.NET.Core.Service;

/// <summary>
/// 微信API客户端
/// </summary>
public partial class WechatApiHttpClientFactory : ISingleton
{
    public readonly WechatOptions _wechatOptions;

    public WechatApiHttpClientFactory(IOptions<WechatOptions> wechatOptions,
        System.Net.Http.IHttpClientFactory httpClientFactory)
    {
        _wechatOptions = wechatOptions.Value;

        FlurlHttp.GlobalSettings.FlurlClientFactory = new DelegatingFlurlClientFactory(httpClientFactory);
    }

    /// <summary>
    /// 微信公众号
    /// </summary>
    /// <returns></returns>
    public WechatApiClient CreateWechatClient()
    {
        if (string.IsNullOrEmpty(_wechatOptions.WechatAppId) || string.IsNullOrEmpty(_wechatOptions.WechatAppSecret))
            throw Oops.Oh("微信公众号配置错误");

        var wechatApiClient = new WechatApiClient(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WechatAppId,
            AppSecret = _wechatOptions.WechatAppSecret,
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
        if (string.IsNullOrEmpty(_wechatOptions.WxOpenAppId) || string.IsNullOrEmpty(_wechatOptions.WxOpenAppSecret))
            throw Oops.Oh("微信小程序配置错误");

        var WechatApiClient = new WechatApiClient(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WxOpenAppId,
            AppSecret = _wechatOptions.WxOpenAppSecret
        });

        WechatApiClient.Configure(settings =>
        {
            settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
        });

        return WechatApiClient;
    }
}

public partial class WechatApiHttpClientFactory
{
    internal class DelegatingFlurlClientFactory : IFlurlClientFactory
    {
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;

        public DelegatingFlurlClientFactory(System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public IFlurlClient Get(Url url)
        {
            return new FlurlClient(_httpClientFactory.CreateClient(url.ToUri().Host));
        }

        public void Dispose()
        {
            // Do Nothing
        }
    }
}