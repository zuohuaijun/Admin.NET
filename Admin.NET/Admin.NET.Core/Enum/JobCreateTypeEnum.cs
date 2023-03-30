namespace Admin.NET.Core;

/// <summary>
/// 作业创建类型枚举
/// </summary>
[Description("作业创建类型枚举")]
public enum JobCreateTypeEnum
{
    /// <summary>
    /// 内置
    /// </summary>
    [Description("内置")]
    BuiltIn = 0,

    /// <summary>
    /// 脚本
    /// </summary>
    [Description("脚本")]
    Script = 1,

    /// <summary>
    /// HTTP请求
    /// </summary>
    [Description("HTTP请求")]
    Http = 2,
}