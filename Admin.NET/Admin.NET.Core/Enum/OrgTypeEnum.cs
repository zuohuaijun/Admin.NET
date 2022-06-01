namespace Admin.NET.Core;

/// <summary>
/// 机构类型枚举
/// </summary>
public enum OrgTypeEnum
{
    /// <summary>
    /// 品牌
    /// </summary>
    [Description("品牌")]
    Brand,

    /// <summary>
    /// 总店(加盟/直营)
    /// </summary>
    [Description("总店(加盟/直营)")]
    Head,

    /// <summary>
    /// 直营店
    /// </summary>
    [Description("直营店")]
    Direct,

    /// <summary>
    /// 加盟店
    /// </summary>
    [Description("加盟店")]
    Franchised
}