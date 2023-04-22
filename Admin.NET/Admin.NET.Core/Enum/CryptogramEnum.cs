namespace Admin.NET.Core;

/// <summary>
/// 密码加密枚举
/// </summary>
[Description("密码加密枚举")]
public enum CryptogramEnum
{
    /// <summary>
    /// MD5
    /// </summary>
    [Description("MD5")]
    MD5 = 0,

    /// <summary>
    /// SM2（国密）
    /// </summary>
    [Description("SM2")]
    SM2 = 1,

    /// <summary>
    /// SM4（国密）
    /// </summary>
    [Description("SM4")]
    SM4 = 2
}