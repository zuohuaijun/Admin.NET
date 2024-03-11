// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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