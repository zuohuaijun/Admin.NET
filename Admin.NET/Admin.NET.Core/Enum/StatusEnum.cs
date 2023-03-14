namespace Admin.NET.Core;

/// <summary>
/// 通用状态枚举
/// </summary>
[Description("通用状态枚举")]
public enum StatusEnum
{
    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable = 1,

    /// <summary>
    /// 停用
    /// </summary>
    [Description("停用")]
    Disable = 2,
}