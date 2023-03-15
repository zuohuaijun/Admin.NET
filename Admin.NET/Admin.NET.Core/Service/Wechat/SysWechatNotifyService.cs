using SKIT.FlurlHttpClient.Wechat.Api.Events;

namespace Admin.NET.Core.Service;

/// <summary>
/// 微信通知服务
/// </summary>
[ApiDescriptionSettings(Order = 250)]
public class SysWechatNotifyService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;
    private readonly WechatApiClient _wechatApiClient;

    public SysWechatNotifyService(SqlSugarRepository<SysWechatUser> sysWechatUserRep,
        WechatApiHttpClient wechatApiHttpClient)
    {
        _sysWechatUserRep = sysWechatUserRep;
        _wechatApiClient = wechatApiHttpClient.CreateWxOpenClient();
    }

    public string VerifyMessage(string appId, string timestamp, string nonce, string signature, string echoString)
    {
        // 验证服务器推送
        // 文档：https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Access_Overview.html

        bool valid = _wechatApiClient.VerifyEventSignatureForEcho(callbackTimestamp: timestamp, callbackNonce: nonce, callbackSignature: signature);
        if (!valid)
        {
            return "fail";
        }

        return echoString;
    }

    public async Task<string> ReceiveMessage(string appId)
    {
        // 接收服务器推送
        // 文档：https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_event_pushes.html

        using var reader = new StreamReader(App.HttpContext.Request.Body, Encoding.UTF8);
        string content = await reader.ReadToEndAsync();

        var msgType = _wechatApiClient.DeserializeEventFromXml(content).MessageType?.ToUpper();
        switch (msgType)
        {
            case "TEXT":
                {
                    var eventModel = _wechatApiClient.DeserializeEventFromXml<TextMessageEvent>(content);
                    //_logger.LogInformation("接收到微信推送的文本消息，消息内容：{0}", eventModel.Content);
                    // 后续处理略
                }
                break;

            case "IMAGE":
                {
                    var eventModel = _wechatApiClient.DeserializeEventFromXml<ImageMessageEvent>(content);
                    //_logger.LogInformation("接收到微信推送的图片消息，图片链接：{0}", eventModel.PictureUrl);
                    // 后续处理略
                }
                break;

            default:
                {
                    // 其他情况略
                }
                break;
        }

        return "success";
    }
}