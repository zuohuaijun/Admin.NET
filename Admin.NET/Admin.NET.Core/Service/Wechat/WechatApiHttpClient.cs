// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using Newtonsoft.Json;

namespace Admin.NET.Core.Service;

/// <summary>
/// 微信API客户端
/// </summary>
public partial class WechatApiClientFactory : ISingleton
{
    private readonly IHttpClientFactory _httpClientFactory;
    public readonly WechatOptions _wechatOptions;

    public WechatApiClientFactory(IHttpClientFactory httpClientFactory, IOptions<WechatOptions> wechatOptions)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _wechatOptions = wechatOptions.Value ?? throw new ArgumentNullException(nameof(wechatOptions));
    }

    /// <summary>
    /// 微信公众号
    /// </summary>
    /// <returns></returns>
    public WechatApiClient CreateWechatClient()
    {
        if (string.IsNullOrEmpty(_wechatOptions.WechatAppId) || string.IsNullOrEmpty(_wechatOptions.WechatAppSecret))
            throw Oops.Oh("微信公众号配置错误");

        var client = WechatApiClientBuilder.Create(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WechatAppId,
            AppSecret = _wechatOptions.WechatAppSecret,
        })
            .UseHttpClient(_httpClientFactory.CreateClient(), disposeClient: false) // 设置 HttpClient 不随客户端一同销毁
            .Build();

        client.Configure(config =>
        {
            JsonSerializerSettings jsonSerializerSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            config.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings); // 指定 System.Text.Json JSON序列化
            // config.JsonSerializer = new SystemTextJsonSerializer(jsonSerializerOptions); // 指定 Newtonsoft.Json  JSON序列化
        });

        return client;
    }

    /// <summary>
    /// 微信小程序
    /// </summary>
    /// <returns></returns>
    public WechatApiClient CreateWxOpenClient()
    {
        if (string.IsNullOrEmpty(_wechatOptions.WxOpenAppId) || string.IsNullOrEmpty(_wechatOptions.WxOpenAppSecret))
            throw Oops.Oh("微信小程序配置错误");

        var client = WechatApiClientBuilder.Create(new WechatApiClientOptions()
        {
            AppId = _wechatOptions.WxOpenAppId,
            AppSecret = _wechatOptions.WxOpenAppSecret
        })
        .UseHttpClient(_httpClientFactory.CreateClient(), disposeClient: false) // 设置 HttpClient 不随客户端一同销毁
        .Build();

        client.Configure(config =>
        {
            JsonSerializerSettings jsonSerializerSettings = NewtonsoftJsonSerializer.GetDefaultSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            config.JsonSerializer = new NewtonsoftJsonSerializer(jsonSerializerSettings); // 指定 System.Text.Json JSON序列化
            // config.JsonSerializer = new SystemTextJsonSerializer(jsonSerializerOptions); // 指定 Newtonsoft.Json  JSON序列化
        });

        return client;
    }
}