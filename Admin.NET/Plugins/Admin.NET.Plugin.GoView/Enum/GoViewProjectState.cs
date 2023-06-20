namespace Admin.NET.Plugin.GoView.Enum;

/// <summary>
/// GoView 项目状态
/// </summary>
[Description("GoView 项目状态")]
public enum GoViewProjectState
{
    /// <summary>
    /// 未发布
    /// </summary>
    [Description("未发布")]
    UnPublish = -1,

    /// <summary>
    /// 已发布
    /// </summary>
    [Description("已发布")]
    Published = 1,
}