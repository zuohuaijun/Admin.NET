namespace Admin.NET.Core;

/// <summary>
/// 示例-类型下拉框
/// </summary>
[ConstSelector("订单状态")]
public class OrderStatus
{
    public const string 待支付 = "待支付1";
    public const string 已支付 = "已支付2";
    public const string 已发货 = "已发货3";
    public const string 已签收 = "已签收4";
    public const string 已评价 = "已评价5";
}

/// <summary>
/// 示例-枚举下拉框
/// </summary>
[ConstSelector("同步状态")]
public enum SyncStatusEnum
{
    /// <summary>
    /// 未同步
    /// </summary>
    [Description("未同步")]
    未同步 = 0,

    /// <summary>
    /// 同步成功
    /// </summary>
    [Description("同步成功")]
    同步成功 = 1,

    /// <summary>
    /// 同步失败
    /// </summary>
    [Description("同步失败")]
    同步失败 = 2
}