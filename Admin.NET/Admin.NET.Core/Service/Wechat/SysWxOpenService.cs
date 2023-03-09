namespace Admin.NET.Core.Service;

/// <summary>
/// 微信小程序服务
/// </summary>
[ApiDescriptionSettings(Order = 250)]
public class SysWxOpenService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;
    private readonly WechatApiClient _wechatApiClient;

    public SysWxOpenService(SqlSugarRepository<SysWechatUser> sysWechatUserRep,
        WechatApiHttpClient wechatApiHttpClient)
    {
        _sysWechatUserRep = sysWechatUserRep;
        _wechatApiClient = wechatApiHttpClient.CreateWxOpenClient();
    }

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="input"></param>
    [AllowAnonymous]
    [DisplayName("获取微信用户OpenId")]
    public async Task<WxOpenIdOutput> GetWxOpenId([FromQuery] JsCode2SessionInput input)
    {
        var reqJsCode2Session = new SnsJsCode2SessionRequest()
        {
            JsCode = input.JsCode,
        };
        var resCode2Session = await _wechatApiClient.ExecuteSnsJsCode2SessionAsync(reqJsCode2Session);
        if (resCode2Session.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resCode2Session.ErrorMessage + " " + resCode2Session.ErrorCode);

        var wxUser = await _sysWechatUserRep.GetFirstAsync(p => p.OpenId == resCode2Session.OpenId);
        if (wxUser == null)
        {
            wxUser = new SysWechatUser
            {
                OpenId = resCode2Session.OpenId,
                UnionId = resCode2Session.UnionId,
                SessionKey = resCode2Session.SessionKey,
                PlatformType = PlatformTypeEnum.微信小程序
            };
            wxUser = await _sysWechatUserRep.AsInsertable(wxUser).ExecuteReturnEntityAsync();
        }
        else
        {
            await _sysWechatUserRep.AsUpdateable(wxUser).IgnoreColumns(true).ExecuteCommandAsync();
        }

        return new WxOpenIdOutput
        {
            OpenId = resCode2Session.OpenId
        };
    }

    /// <summary>
    /// 获取微信用户电话号码
    /// </summary>
    /// <param name="input"></param>
    [AllowAnonymous]
    [DisplayName("获取微信用户电话号码")]
    public async Task<WxPhoneOutput> GetWxPhone([FromQuery] WxPhoneInput input)
    {
        var reqCgibinToken = new CgibinTokenRequest();
        var resCgibinToken = await _wechatApiClient.ExecuteCgibinTokenAsync(reqCgibinToken);
        if (resCgibinToken.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resCgibinToken.ErrorMessage + " " + resCgibinToken.ErrorCode);

        var reqUserPhoneNumber = new WxaBusinessGetUserPhoneNumberRequest()
        {
            Code = input.Code,
            AccessToken = resCgibinToken.AccessToken,
        };
        var resUserPhoneNumber = await _wechatApiClient.ExecuteWxaBusinessGetUserPhoneNumberAsync(reqUserPhoneNumber);
        if (resUserPhoneNumber.ErrorCode != (int)WechatReturnCodeEnum.请求成功)
            throw Oops.Oh(resUserPhoneNumber.ErrorMessage + " " + resUserPhoneNumber.ErrorCode);

        return new WxPhoneOutput
        {
            PhoneNumber = resUserPhoneNumber.PhoneInfo?.PhoneNumber
        };
    }

    /// <summary>
    /// 微信小程序登录OpenId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("微信小程序登录OpenId")]
    public async Task<dynamic> WxOpenIdLogin(WxOpenIdLoginInput input)
    {
        var wxUser = await _sysWechatUserRep.GetFirstAsync(p => p.OpenId == input.OpenId);
        if (wxUser == null)
            throw Oops.Oh("微信小程序登录失败");
        return new
        {
            wxUser.Avatar,
            accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimConst.UserId, wxUser.Id },
                { ClaimConst.OpenId, wxUser.OpenId },
                { ClaimConst.RealName, wxUser.NickName },
                { ClaimConst.LoginMode, LoginModeEnum.APP },
            })
        };
    }
}