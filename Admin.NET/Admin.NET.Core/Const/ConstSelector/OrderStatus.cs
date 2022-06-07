namespace Admin.NET.Core.ConstSelector;

/// <summary>
/// 示例 类型下拉框
/// </summary>
[EnumSelector("订单状态")]
public class OrderStatus
{
    public const string 待支付 = "待支付1";
    public const string 已支付 = "已支付2";
    public const string 已发货 = "已发货3";
    public const string 已签收 = "已签收4";
    public const string 已评价 = "已评价5";
}

