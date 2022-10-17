namespace Admin.NET.Core.Service;

/// <summary>
/// 微信支付服务
/// </summary>
[ApiDescriptionSettings(Order = 99)]
public class WeChatPayService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WeChatPay> _weChatPayUserRep;

    private readonly WeChatPayOptions _weChatPayOptions;
    public readonly WechatTenpayClient WeChatTenpayClient;
    private readonly PayCallBackOptions _payCallBackOptions;

    public WeChatPayService(SqlSugarRepository<WeChatPay> weChatPayUserRep,
        IOptions<WeChatPayOptions> weChatPayOptions,
        IOptions<PayCallBackOptions> payCallBackOptions)
    {
        _weChatPayUserRep = weChatPayUserRep;
        _weChatPayOptions = weChatPayOptions.Value;
        WeChatTenpayClient = CreateTenpayClient();
        _payCallBackOptions = payCallBackOptions.Value;
    }

    /// <summary>
    /// 初始化微信支付客户端
    /// </summary>
    /// <returns></returns>
    private WechatTenpayClient CreateTenpayClient()
    {
        var tenpayClientOptions = new WechatTenpayClientOptions()
        {
            MerchantId = _weChatPayOptions.MerchantId,
            MerchantV3Secret = _weChatPayOptions.MerchantV3Secret,
            MerchantCertificateSerialNumber = _weChatPayOptions.MerchantCertificateSerialNumber,
            MerchantCertificatePrivateKey = File.ReadAllText(App.WebHostEnvironment.ContentRootPath + _weChatPayOptions.MerchantCertificatePrivateKey),
            PlatformCertificateManager = new InMemoryCertificateManager()
        };
        return new WechatTenpayClient(tenpayClientOptions);
    }

    /// <summary>
    /// 生成JSAPI调起支付所需参数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatPay/genPayPara")]
    public dynamic GenerateParametersForJsapiPay(WeChatPayParaInput input)
    {
        return WeChatTenpayClient.GenerateParametersForJsapiPayRequest(_weChatPayOptions.AppId, input.PrepayId);
    }

    /// <summary>
    /// 微信支付统一下单获取Id(商户直连)
    /// </summary>
    [HttpPost("/weChatPay/payTransaction")]
    public async Task<dynamic> CreatePayTransaction([FromBody] WeChatPayTransactionInput input)
    {
        var request = new CreatePayTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _weChatPayOptions.AppId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WeChatPayUrl,
            Amount = new CreatePayTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await WeChatTenpayClient.ExecuteCreatePayTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw Oops.Oh(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new WeChatPay()
        {
            MerchantId = _weChatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId
        };
        await _weChatPayUserRep.InsertAsync(wechatPay);

        return new
        {
            response.PrepayId,
            request.OutTradeNumber
        };
    }

    /// <summary>
    /// 微信支付统一下单获取Id(服务商模式)
    /// </summary>
    [HttpPost("/weChatPay/payPartnerTransaction")]
    public async Task<dynamic> CreatePayPartnerTransaction([FromBody] WeChatPayTransactionInput input)
    {
        var request = new CreatePayPartnerTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _weChatPayOptions.AppId,
            MerchantId = _weChatPayOptions.MerchantId,
            SubAppId = _weChatPayOptions.AppId,
            SubMerchantId = _weChatPayOptions.MerchantId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WeChatPayUrl,
            Amount = new CreatePayPartnerTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayPartnerTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await WeChatTenpayClient.ExecuteCreatePayPartnerTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw Oops.Oh(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new WeChatPay()
        {
            AppId = _weChatPayOptions.AppId,
            MerchantId = _weChatPayOptions.MerchantId,
            SubAppId = _weChatPayOptions.AppId,
            SubMerchantId = _weChatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId
        };
        await _weChatPayUserRep.InsertAsync(wechatPay);

        return new
        {
            response.PrepayId,
            request.OutTradeNumber
        };
    }

    /// <summary>
    /// 获取支付订单详情
    /// </summary>
    /// <param name="tradeId"></param>
    /// <returns></returns>
    [HttpGet("/weChatPay/payInfo")]
    public async Task<WeChatPay> GetWeChatPayInfo(string tradeId)
    {
        return await _weChatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == tradeId);
    }

    /// <summary>
    /// 微信支付成功回调(商户直连)
    /// </summary>
    /// <returns></returns>
    [HttpPost("/notify/weChatPay/payCallBack")]
    [AllowAnonymous]
    public async Task<WeChatPayOutput> WeChatPayCallBack()
    {
        using var ms = new MemoryStream();
        await App.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = WeChatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = WeChatTenpayClient.DecryptEventResource<TransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await _weChatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
                && u.MerchantId == callbackResource.MerchantId);
            if (wechatPay == null) return null;
            //wechatPay.OpenId = callbackResource.Payer.OpenId; // 支付者标识
            //wechatPay.MerchantId = callbackResource.MerchantId; // 微信商户号
            //wechatPay.OutTradeNumber = callbackResource.OutTradeNumber; // 商户订单号
            wechatPay.TransactionId = callbackResource.TransactionId; // 支付订单号
            wechatPay.TradeType = callbackResource.TradeType; // 交易类型
            wechatPay.TradeState = callbackResource.TradeState; // 交易状态
            wechatPay.TradeStateDescription = callbackResource.TradeStateDescription; // 交易状态描述
            wechatPay.BankType = callbackResource.BankType; // 付款银行类型
            wechatPay.Total = callbackResource.Amount.Total; // 订单总金额
            wechatPay.PayerTotal = callbackResource.Amount.PayerTotal; // 用户支付金额
            wechatPay.SuccessTime = callbackResource.SuccessTime; // 支付完成时间

            await _weChatPayUserRep.AsUpdateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();

            return new WeChatPayOutput()
            {
                Total = wechatPay.Total,
                Attachment = wechatPay.Attachment,
                GoodsTag = wechatPay.GoodsTag
            };
        }

        return null;
    }

    /// <summary>
    /// 微信支付成功回调(服务商模式)
    /// </summary>
    /// <returns></returns>
    [HttpPost("/notify/weChatPay/payPartnerCallback")]
    [AllowAnonymous]
    public async Task WeChatPayPartnerCallBack()
    {
        using var ms = new MemoryStream();
        await App.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = WeChatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = WeChatTenpayClient.DecryptEventResource<PartnerTransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await _weChatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
                && u.MerchantId == callbackResource.MerchantId);
            if (wechatPay == null) return;
            //wechatPay.OpenId = callbackResource.Payer.OpenId; // 支付者标识
            //wechatPay.MerchantId = callbackResource.MerchantId; // 微信商户号
            //wechatPay.OutTradeNumber = callbackResource.OutTradeNumber; // 商户订单号
            wechatPay.TransactionId = callbackResource.TransactionId; // 支付订单号
            wechatPay.TradeType = callbackResource.TradeType; // 交易类型
            wechatPay.TradeState = callbackResource.TradeState; // 交易状态
            wechatPay.TradeStateDescription = callbackResource.TradeStateDescription; // 交易状态描述
            wechatPay.BankType = callbackResource.BankType; // 付款银行类型
            wechatPay.Total = callbackResource.Amount.Total; // 订单总金额
            wechatPay.PayerTotal = callbackResource.Amount.PayerTotal; // 用户支付金额
            wechatPay.SuccessTime = callbackResource.SuccessTime; // 支付完成时间

            await _weChatPayUserRep.AsUpdateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();
        }
    }
}