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
/// 文化程度枚举
/// </summary>
[Description("文化程度枚举")]
public enum CultureLevelEnum
{
    /// <summary>
    /// 小学
    /// </summary>
    [Description("小学")]
    Level1 = 0,

    /// <summary>
    /// 初中
    /// </summary>
    [Description("初中")]
    Level2 = 1,

    /// <summary>
    /// 高中
    /// </summary>
    [Description("高中")]
    Level3 = 2,

    /// <summary>
    /// 中专
    /// </summary>
    [Description("中专")]
    Level4 = 3,

    /// <summary>
    /// 大专
    /// </summary>
    [Description("大专")]
    Level5 = 4,

    /// <summary>
    /// 本科
    /// </summary>
    [Description("本科")]
    Level6 = 5,

    /// <summary>
    /// 硕士研究生
    /// </summary>
    [Description("硕士研究生")]
    Level7 = 6,

    /// <summary>
    /// 博士研究生
    /// </summary>
    [Description("博士研究生")]
    Level8 = 7,
}