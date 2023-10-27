// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 微信公众号服务
/// </summary>
[ApiDescriptionSettings(Order = 230)]
public class SysWechatService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;
    private readonly SysConfigService _sysConfigService;
    private readonly WechatApiHttpClientFactory _wechatApiHttpClientFactory;
    private readonly WechatApiClient _wechatApiClient;

    public SysWechatService(SqlSugarRepository<SysWechatUser> sysWechatUserRep,
        SysConfigService sysConfigService,
        WechatApiHttpClientFactory wechatApiHttpClientFactory)
    {
        _sysWechatUserRep = sysWechatUserRep;
        _sysConfigService = sysConfigService;
        _wechatApiHttpClientFactory = wechatApiHttpClientFactory;
        _wechatApiClient = wechatApiHttpClientFactory.CreateWechatClient();
    }

    /// <summary>
    /// 生成网页授权Url
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("生成网页授权Url")]
    public string GenAuthUrl(GenAuthUrlInput input)
    {
        return _wechatApiClient.GenerateParameterizedUrlForConnectOAuth2Authorize(input.RedirectUrl, input.Scope, null);
    }

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    [AllowAnonymous]
    [DisplayName("获取微信用户OpenId")]
    public async Task<string> SnsOAuth2([FromQuery] WechatOAuth2Input input)
    {
        var reqOAuth2 = new SnsOAuth2AccessTokenRequest()
        {
            Code = input.Code,
        };
        var resOAuth2 = await _wechatApiClient.ExecuteSnsOAuth2AccessTokenAsync(reqOAuth2);
        if (resOAuth2.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resOAuth2.ErrorMessage + " " + resOAuth2.ErrorCode);

        var wxUser = await _sysWechatUserRep.GetFirstAsync(p => p.OpenId == resOAuth2.OpenId);
        if (wxUser == null)
        {
            var reqUserInfo = new SnsUserInfoRequest()
            {
                OpenId = resOAuth2.OpenId,
                AccessToken = resOAuth2.AccessToken,
            };
            var resUserInfo = await _wechatApiClient.ExecuteSnsUserInfoAsync(reqUserInfo);
            wxUser = resUserInfo.Adapt<SysWechatUser>();
            wxUser.Avatar = resUserInfo.HeadImageUrl;
            wxUser.NickName = resUserInfo.Nickname;
            wxUser = await _sysWechatUserRep.AsInsertable(wxUser).ExecuteReturnEntityAsync();
        }
        else
        {
            wxUser.AccessToken = resOAuth2.AccessToken;
            wxUser.RefreshToken = resOAuth2.RefreshToken;
            await _sysWechatUserRep.AsUpdateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
        }

        return resOAuth2.OpenId;
    }

    /// <summary>
    /// 微信用户登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("微信用户登录OpenId")]
    public async Task<dynamic> OpenIdLogin(WechatUserLogin input)
    {
        var wxUser = await _sysWechatUserRep.GetFirstAsync(p => p.OpenId == input.OpenId);
        if (wxUser == null)
            throw Oops.Oh("微信用户登录OpenId错误");

        var tokenExpire = await _sysConfigService.GetTokenExpire();
        return new
        {
            wxUser.Avatar,
            accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimConst.UserId, wxUser.Id },
                { ClaimConst.NickName, wxUser.NickName },
                { ClaimConst.LoginMode, LoginModeEnum.APP },
            }, tokenExpire)
        };
    }

    /// <summary>
    /// 获取配置签名参数(wx.config)
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取配置签名参数(wx.config)")]
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

    /// <summary>
    /// 获取模板列表
    /// </summary>
    [DisplayName("获取模板列表")]
    public async Task<dynamic> GetMessageTemplateList()
    {
        var accessToken = await GetCgibinToken();
        var reqTemplate = new CgibinTemplateGetAllPrivateTemplateRequest()
        {
            AccessToken = accessToken
        };
        var resTemplate = await _wechatApiClient.ExecuteCgibinTemplateGetAllPrivateTemplateAsync(reqTemplate);
        if (resTemplate.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resTemplate.ErrorMessage + " " + resTemplate.ErrorCode);

        return resTemplate.TemplateList;
    }

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("发送模板消息")]
    public async Task<dynamic> SendTemplateMessage(MessageTemplateSendInput input)
    {
        var dataInfo = input.Data.ToDictionary(k => k.Key, k => k.Value);
        var messageData = new Dictionary<string, CgibinMessageTemplateSendRequest.Types.DataItem>();
        foreach (var item in dataInfo)
        {
            messageData.Add(item.Key, new CgibinMessageTemplateSendRequest.Types.DataItem() { Value = "" + item.Value.Value.ToString() + "" });
        }

        var accessToken = await GetCgibinToken();
        var reqMessage = new CgibinMessageTemplateSendRequest()
        {
            AccessToken = accessToken,
            TemplateId = input.TemplateId,
            ToUserOpenId = input.ToUserOpenId,
            Url = input.Url,
            MiniProgram = new CgibinMessageTemplateSendRequest.Types.MiniProgram
            {
                AppId = _wechatApiHttpClientFactory._wechatOptions.WechatAppId,
                PagePath = input.MiniProgramPagePath,
            },
            Data = messageData
        };
        var resMessage = await _wechatApiClient.ExecuteCgibinMessageTemplateSendAsync(reqMessage);
        return resMessage;
    }

    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DeleteMessageTemplate"), HttpPost]
    [DisplayName("删除模板")]
    public async Task<dynamic> DeleteMessageTemplate(DeleteMessageTemplateInput input)
    {
        var accessToken = await GetCgibinToken();
        var reqMessage = new CgibinTemplateDeletePrivateTemplateRequest()
        {
            AccessToken = accessToken,
            TemplateId = input.TemplateId
        };
        var resTemplate = await _wechatApiClient.ExecuteCgibinTemplateDeletePrivateTemplateAsync(reqMessage);
        return resTemplate;
    }

    /// <summary>
    /// 获取Access_token
    /// </summary>
    private async Task<string> GetCgibinToken()
    {
        var reqCgibinToken = new CgibinTokenRequest();
        var resCgibinToken = await _wechatApiClient.ExecuteCgibinTokenAsync(reqCgibinToken);
        if (resCgibinToken.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resCgibinToken.ErrorMessage + " " + resCgibinToken.ErrorCode);
        return resCgibinToken.AccessToken;
    }
}