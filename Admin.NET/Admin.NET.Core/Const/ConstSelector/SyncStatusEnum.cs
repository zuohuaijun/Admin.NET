namespace Admin.NET.Core.ConstSelector;
/// <summary>
/// 示例 枚举下拉框
/// </summary>
[EnumSelector("同步状态")]
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