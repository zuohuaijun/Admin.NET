// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Plugin.GoView.Entity;

/// <summary>
/// GoView 项目表
/// </summary>
[SugarTable(null, "GoView 项目表")]
public class GoViewPro : EntityTenant
{
    /// <summary>
    /// 项目名称
    /// </summary>
    [SugarColumn(ColumnDescription = "项目名称", Length = 64)]
    [Required, MaxLength(64)]
    public string ProjectName { get; set; }

    /// <summary>
    /// 项目状态
    /// </summary>
    [SugarColumn(ColumnDescription = "项目状态")]
    public GoViewProState State { get; set; } = GoViewProState.UnPublish;

    /// <summary>
    /// 预览图片Url
    /// </summary>
    [SugarColumn(ColumnDescription = "预览图片Url", Length = 1024)]
    [MaxLength(1024)]
    public string? IndexImage { get; set; }

    /// <summary>
    /// 项目备注
    /// </summary>
    [SugarColumn(ColumnDescription = "项目备注", Length = 512)]
    [MaxLength(512)]
    public string? Remarks { get; set; }

    ///// <summary>
    ///// 项目数据
    ///// </summary>
    //[Navigate(NavigateType.OneToOne, nameof(Id))]
    //public GoViewProData GoViewProData { get; set; }
}