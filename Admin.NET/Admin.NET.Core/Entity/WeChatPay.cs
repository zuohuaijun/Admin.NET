namespace Admin.NET.Core;

/// <summary>
/// 微信支付表
/// </summary>
[SugarTable("wechat_pay", "微信支付表")]
public class WeChatPay : EntityBase
{
    /// <summary>
    /// 微信商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "微信商户号")]
    public string MerchantId { get; set; }

    /// <summary>
    /// 服务商AppId
    /// </summary>
    [SugarColumn(ColumnDescription = "服务商AppId")]
    public string AppId { get; set; }

    /// <summary>
    /// 商户订单号
    /// </summary>
    [SugarColumn(ColumnDescription = "商户订单号")]
    public string OutTradeNumber { get; set; }

    /// <summary>
    /// 支付订单号
    /// </summary>
    [SugarColumn(ColumnDescription = "支付订单号")]
    public string TransactionId { get; set; }

    /// <summary>
    /// 交易类型
    /// </summary>
    [SugarColumn(ColumnDescription = "交易类型")]
    public string TradeType { get; set; }

    /// <summary>
    /// 交易状态
    /// </summary>
    [SugarColumn(ColumnDescription = "交易状态")]
    public string TradeState { get; set; }

    /// <summary>
    /// 交易状态描述
    /// </summary>
    [SugarColumn(ColumnDescription = "交易状态描述")]
    public string TradeStateDescription { get; set; }

    /// <summary>
    /// 付款银行类型
    /// </summary>
    [SugarColumn(ColumnDescription = "付款银行类型")]
    public string BankType { get; set; }

    /// <summary>
    /// 订单总金额
    /// </summary>
    [SugarColumn(ColumnDescription = "订单总金额")]
    public int Total { get; set; }

    /// <summary>
    /// 用户支付金额
    /// </summary>
    [SugarColumn(ColumnDescription = "用户支付金额")]
    public int? PayerTotal { get; set; }

    /// <summary>
    /// 支付完成时间
    /// </summary>
    [SugarColumn(ColumnDescription = "支付完成时间")]
    public DateTimeOffset? SuccessTime { get; set; }

    /// <summary>
    /// 交易结束时间
    /// </summary>
    [SugarColumn(ColumnDescription = "交易结束时间")]
    public DateTimeOffset? ExpireTime { get; set; }

    /// <summary>
    /// 商品描述
    /// </summary>
    [SugarColumn(ColumnDescription = "商品描述")]
    public string Description { get; set; }

    /// <summary>
    /// 场景信息
    /// </summary>
    [SugarColumn(ColumnDescription = "场景信息")]
    public string Scene { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    [SugarColumn(ColumnDescription = "附加数据")]
    public string Attachment { get; set; }

    /// <summary>
    /// 优惠标记
    /// </summary>
    [SugarColumn(ColumnDescription = "优惠标记")]
    public string GoodsTag { get; set; }

    /// <summary>
    /// 结算信息
    /// </summary>
    [SugarColumn(ColumnDescription = "结算信息")]
    public string Settlement { get; set; }

    /// <summary>
    /// 回调通知地址
    /// </summary>
    [SugarColumn(ColumnDescription = "回调通知地址")]
    public string NotifyUrl { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注")]
    public string Remark { get; set; }

    /// <summary>
    /// 微信OpenId标识
    /// </summary>
    [SugarColumn(ColumnDescription = "微信OpenId标识")]
    public string OpenId { get; set; }

    /// <summary>
    /// 关联微信用户
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public WeChatUser WeChatUser { get; set; }

    /// <summary>
    /// 子商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "子商户号")]
    public string SubMerchantId { get; set; }

    /// <summary>
    /// 子商户AppId
    /// </summary>
    [SugarColumn(ColumnDescription = "回调通知地址")]
    public string SubAppId { get; set; }

    /// <summary>
    /// 子商户唯一标识
    /// </summary>
    [SugarColumn(ColumnDescription = "子商户唯一标识")]
    public string SubOpenId { get; set; }
}