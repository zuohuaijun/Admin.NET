namespace Admin.NET.Core.Service;

/// <summary>
/// 微信公众号服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class WeChatService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WeChatUser> _weChatUserRep;
    private readonly WechatApiClient _weChatApiClient;

    public WeChatService(SqlSugarRepository<WeChatUser> weChatUserRep,
        WeChatApiHttpClient weChatApiHttpClient)
    {
        _weChatUserRep = weChatUserRep;
        _weChatApiClient = weChatApiHttpClient.CreateWeChatClient();
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
        return _weChatApiClient.GenerateParameterizedUrlForConnectOAuth2Authorize(input.RedirectUrl, input.Scope);
    }

    /// <summary>
    /// 授权登录(Code换取OpenId)
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/weChat/snsOAuth2")]
    [AllowAnonymous]
    public async Task<string> LoginOAuth2([Required] WeChatOAuth2Input input)
    {
        var reqOAuth2 = new SnsOAuth2AccessTokenRequest()
        {
            Code = input.Code,
        };
        var resOAuth2 = await _weChatApiClient.ExecuteSnsOAuth2AccessTokenAsync(reqOAuth2);
        if (resOAuth2.ErrorCode != (int)WeChatReturnCodeEnum.请求成功)
            throw Oops.Oh(resOAuth2.ErrorMessage + resOAuth2.ErrorCode);

        var wxUser = await _weChatUserRep.GetFirstAsync(p => p.OpenId == resOAuth2.OpenId);
        if (wxUser == null)
        {
            var reqUserInfo = new SnsUserInfoRequest()
            {
                OpenId = resOAuth2.OpenId,
                AccessToken = resOAuth2.AccessToken,
            };
            var resUserInfo = await _weChatApiClient.ExecuteSnsUserInfoAsync(reqUserInfo);
            wxUser = resUserInfo.Adapt<WeChatUser>();
            wxUser.Avatar = resUserInfo.HeadImageUrl;
            wxUser.NickName = resUserInfo.Nickname;
            wxUser = await _weChatUserRep.AsInsertable(wxUser).ExecuteReturnEntityAsync();
        }
        else
        {
            wxUser.AccessToken = resOAuth2.AccessToken;
            wxUser.RefreshToken = resOAuth2.RefreshToken;
            await _weChatUserRep.AsUpdateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
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
    public async Task<dynamic> WechatUserLogin(WeChatUserLogin input)
    {
        var wxUser = await _weChatUserRep.GetFirstAsync(p => p.OpenId == input.OpenId);
        if (wxUser == null)
            throw Oops.Oh("微信登录");
        return new
        {
            wxUser.Avatar,
            accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimConst.UserId, wxUser.Id },
                { ClaimConst.OpenId, wxUser.OpenId },
                { ClaimConst.NickName, wxUser.NickName },
                { ClaimConst.RunMode, RunModeEnum.OpenID },
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
        var resCgibinToken = await _weChatApiClient.ExecuteCgibinTokenAsync(new CgibinTokenRequest());
        var request = new CgibinTicketGetTicketRequest()
        {
            AccessToken = resCgibinToken.AccessToken
        };
        var response = await _weChatApiClient.ExecuteCgibinTicketGetTicketAsync(request);
        if (!response.IsSuccessful())
            throw Oops.Oh(response.ErrorMessage);
        return _weChatApiClient.GenerateParametersForJSSDKConfig(response.Ticket, input.Url);
    }
}