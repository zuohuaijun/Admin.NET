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
    /// 其他
    /// </summary>
    [Description("其他")]
    Level0 = 0,

    /// <summary>
    /// 小学
    /// </summary>
    [Description("小学")]
    Level1 = 1,

    /// <summary>
    /// 初中
    /// </summary>
    [Description("初中")]
    Level2 = 2,

    /// <summary>
    /// 普通高中
    /// </summary>
    [Description("普通高中")]
    Level3 = 3,

    /// <summary>
    /// 技工学校
    /// </summary>
    [Description("技工学校")]
    Level4 = 4,

    /// <summary>
    /// 职业教育
    /// </summary>
    [Description("职业教育")]
    Level5 = 5,

    /// <summary>
    /// 职业高中
    /// </summary>
    [Description("职业高中")]
    Level6 = 6,

    /// <summary>
    /// 中等专科
    /// </summary>
    [Description("中等专科")]
    Level7 = 7,

    /// <summary>
    /// 大学专科
    /// </summary>
    [Description("大学专科")]
    Level8 = 8,

    /// <summary>
    /// 大学本科
    /// </summary>
    [Description("大学本科")]
    Level9 = 9,

    /// <summary>
    /// 硕士研究生
    /// </summary>
    [Description("硕士研究生")]
    Level10 = 10,

    /// <summary>
    /// 博士研究生
    /// </summary>
    [Description("博士研究生")]
    Level11 = 11,
}