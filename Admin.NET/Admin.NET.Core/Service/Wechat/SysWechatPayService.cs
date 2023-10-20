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
/// 微信支付服务
/// </summary>
[ApiDescriptionSettings(Order = 210)]
public class SysWechatPayService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWechatPay> _sysWechatPayUserRep;

    private readonly WechatPayOptions _wechatPayOptions;
    private readonly PayCallBackOptions _payCallBackOptions;

    private readonly WechatTenpayClient _wechatTenpayClient;

    public SysWechatPayService(SqlSugarRepository<SysWechatPay> sysWechatPayUserRep,
        IOptions<WechatPayOptions> wechatPayOptions,
        IOptions<PayCallBackOptions> payCallBackOptions)
    {
        _sysWechatPayUserRep = sysWechatPayUserRep;
        _wechatPayOptions = wechatPayOptions.Value;
        _payCallBackOptions = payCallBackOptions.Value;

        _wechatTenpayClient = CreateTenpayClient();
    }

    /// <summary>
    /// 初始化微信支付客户端
    /// </summary>
    /// <returns></returns>
    private WechatTenpayClient CreateTenpayClient()
    {
        var cerFilePath = App.WebHostEnvironment.ContentRootPath + _wechatPayOptions.MerchantCertificatePrivateKey;

        var tenpayClientOptions = new WechatTenpayClientOptions()
        {
            MerchantId = _wechatPayOptions.MerchantId,
            MerchantV3Secret = _wechatPayOptions.MerchantV3Secret,
            MerchantCertificateSerialNumber = _wechatPayOptions.MerchantCertificateSerialNumber,
            MerchantCertificatePrivateKey = File.Exists(cerFilePath) ? File.ReadAllText(cerFilePath) : "",
            PlatformCertificateManager = new InMemoryCertificateManager()
        };
        return new WechatTenpayClient(tenpayClientOptions);
    }

    /// <summary>
    /// 生成JSAPI调起支付所需参数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("生成JSAPI调起支付所需参数")]
    public dynamic GenerateParametersForJsapiPay(WechatPayParaInput input)
    {
        return _wechatTenpayClient.GenerateParametersForJsapiPayRequest(_wechatPayOptions.AppId, input.PrepayId);
    }

    /// <summary>
    /// 微信支付统一下单获取Id(商户直连)
    /// </summary>
    [DisplayName("微信支付统一下单获取Id(商户直连)")]
    public async Task<dynamic> CreatePayTransaction([FromBody] WechatPayTransactionInput input)
    {
        var request = new CreatePayTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _wechatPayOptions.AppId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WechatPayUrl,
            Amount = new CreatePayTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await _wechatTenpayClient.ExecuteCreatePayTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw Oops.Oh(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new SysWechatPay()
        {
            AppId = _wechatPayOptions.AppId,
            MerchantId = _wechatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId,
            TransactionId = ""
        };
        await _sysWechatPayUserRep.InsertAsync(wechatPay);

        return new
        {
            response.PrepayId,
            request.OutTradeNumber
        };
    }

    /// <summary>
    /// 微信支付统一下单获取Id(服务商模式)
    /// </summary>
    [DisplayName("微信支付统一下单获取Id(服务商模式)")]
    public async Task<dynamic> CreatePayPartnerTransaction([FromBody] WechatPayTransactionInput input)
    {
        var request = new CreatePayPartnerTransactionJsapiRequest()
        {
            OutTradeNumber = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(100, 1000), // 订单号
            AppId = _wechatPayOptions.AppId,
            MerchantId = _wechatPayOptions.MerchantId,
            SubAppId = _wechatPayOptions.AppId,
            SubMerchantId = _wechatPayOptions.MerchantId,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            ExpireTime = DateTimeOffset.Now.AddMinutes(10),
            NotifyUrl = _payCallBackOptions.WechatPayUrl,
            Amount = new CreatePayPartnerTransactionJsapiRequest.Types.Amount() { Total = input.Total },
            Payer = new CreatePayPartnerTransactionJsapiRequest.Types.Payer() { OpenId = input.OpenId }
        };
        var response = await _wechatTenpayClient.ExecuteCreatePayPartnerTransactionJsapiAsync(request);
        if (!response.IsSuccessful())
            throw Oops.Oh(response.ErrorMessage);

        // 保存订单信息
        var wechatPay = new SysWechatPay()
        {
            AppId = _wechatPayOptions.AppId,
            MerchantId = _wechatPayOptions.MerchantId,
            SubAppId = _wechatPayOptions.AppId,
            SubMerchantId = _wechatPayOptions.MerchantId,
            OutTradeNumber = request.OutTradeNumber,
            Description = input.Description,
            Attachment = input.Attachment,
            GoodsTag = input.GoodsTag,
            Total = input.Total,
            OpenId = input.OpenId,
            TransactionId = ""
        };
        await _sysWechatPayUserRep.InsertAsync(wechatPay);

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
    [DisplayName("获取支付订单详情")]
    public async Task<SysWechatPay> GetPayInfo(string tradeId)
    {
        return await _sysWechatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == tradeId);
    }

    /// <summary>
    /// 微信支付成功回调(商户直连)
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("微信支付成功回调(商户直连)")]
    public async Task<WechatPayOutput> PayCallBack()
    {
        using var ms = new MemoryStream();
        await App.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = _wechatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = _wechatTenpayClient.DecryptEventResource<TransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await _sysWechatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
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

            await _sysWechatPayUserRep.AsUpdateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();

            return new WechatPayOutput()
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
    [AllowAnonymous]
    [DisplayName("微信支付成功回调(服务商模式)")]
    public async Task PayPartnerCallBack()
    {
        using var ms = new MemoryStream();
        await App.HttpContext.Request.Body.CopyToAsync(ms);
        var b = ms.ToArray();
        var callbackJson = Encoding.UTF8.GetString(b);

        var callbackModel = _wechatTenpayClient.DeserializeEvent(callbackJson);
        if ("TRANSACTION.SUCCESS".Equals(callbackModel.EventType))
        {
            var callbackResource = _wechatTenpayClient.DecryptEventResource<PartnerTransactionResource>(callbackModel);

            // 修改订单支付状态
            var wechatPay = await _sysWechatPayUserRep.GetFirstAsync(u => u.OutTradeNumber == callbackResource.OutTradeNumber
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

            await _sysWechatPayUserRep.AsUpdateable(wechatPay).IgnoreColumns(true).ExecuteCommandAsync();
        }
    }
}