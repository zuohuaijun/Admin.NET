// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 系统微信支付表
/// </summary>
[SugarTable(null, "系统微信支付表")]
[SysTable]
public class SysWechatPay : EntityBase
{
    /// <summary>
    /// 微信商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "微信商户号")]
    [Required]
    public virtual string MerchantId { get; set; }

    /// <summary>
    /// 服务商AppId
    /// </summary>
    [SugarColumn(ColumnDescription = "服务商AppId")]
    [Required]
    public virtual string AppId { get; set; }

    /// <summary>
    /// 商户订单号
    /// </summary>
    [SugarColumn(ColumnDescription = "商户订单号")]
    [Required]
    public virtual string OutTradeNumber { get; set; }

    /// <summary>
    /// 支付订单号
    /// </summary>
    [SugarColumn(ColumnDescription = "支付订单号")]
    [Required]
    public virtual string TransactionId { get; set; }

    /// <summary>
    /// 交易类型
    /// </summary>
    [SugarColumn(ColumnDescription = "交易类型")]
    public string? TradeType { get; set; }

    /// <summary>
    /// 交易状态
    /// </summary>
    [SugarColumn(ColumnDescription = "交易状态")]
    public string? TradeState { get; set; }

    /// <summary>
    /// 交易状态描述
    /// </summary>
    [SugarColumn(ColumnDescription = "交易状态描述")]
    public string? TradeStateDescription { get; set; }

    /// <summary>
    /// 付款银行类型
    /// </summary>
    [SugarColumn(ColumnDescription = "付款银行类型")]
    public string? BankType { get; set; }

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
    public string? Description { get; set; }

    /// <summary>
    /// 场景信息
    /// </summary>
    [SugarColumn(ColumnDescription = "场景信息")]
    public string? Scene { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    [SugarColumn(ColumnDescription = "附加数据")]
    public string? Attachment { get; set; }

    /// <summary>
    /// 优惠标记
    /// </summary>
    [SugarColumn(ColumnDescription = "优惠标记")]
    public string? GoodsTag { get; set; }

    /// <summary>
    /// 结算信息
    /// </summary>
    [SugarColumn(ColumnDescription = "结算信息")]
    public string? Settlement { get; set; }

    /// <summary>
    /// 回调通知地址
    /// </summary>
    [SugarColumn(ColumnDescription = "回调通知地址")]
    public string? NotifyUrl { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注")]
    public string? Remark { get; set; }

    /// <summary>
    /// 微信OpenId标识
    /// </summary>
    [SugarColumn(ColumnDescription = "微信OpenId标识")]
    public string? OpenId { get; set; }

    /// <summary>
    /// 关联微信用户
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(OpenId))]
    public SysWechatUser SysWechatUser { get; set; }

    /// <summary>
    /// 子商户号
    /// </summary>
    [SugarColumn(ColumnDescription = "子商户号")]
    public string? SubMerchantId { get; set; }

    /// <summary>
    /// 子商户AppId
    /// </summary>
    [SugarColumn(ColumnDescription = "回调通知地址")]
    public string? SubAppId { get; set; }

    /// <summary>
    /// 子商户唯一标识
    /// </summary>
    [SugarColumn(ColumnDescription = "子商户唯一标识")]
    public string? SubOpenId { get; set; }
}