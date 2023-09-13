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
/// 系统通知公告表
/// </summary>
[SugarTable(null, "系统通知公告表")]
[SysTable]
public class SysNotice : EntityBase
{
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnDescription = "标题", Length = 32)]
    [Required, MaxLength(32)]
    [SensitiveDetection('*')]
    public virtual string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(ColumnDescription = "内容", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    [Required]
    [SensitiveDetection('*')]
    public virtual string Content { get; set; }

    /// <summary>
    /// 类型（1通知 2公告）
    /// </summary>
    [SugarColumn(ColumnDescription = "类型（1通知 2公告）")]
    public NoticeTypeEnum Type { get; set; }

    /// <summary>
    /// 发布人Id
    /// </summary>
    [SugarColumn(ColumnDescription = "发布人Id")]
    public long PublicUserId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "发布人姓名", Length = 32)]
    [MaxLength(32)]
    public string? PublicUserName { get; set; }

    /// <summary>
    /// 发布机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "发布机构Id")]
    public long PublicOrgId { get; set; }

    /// <summary>
    /// 发布机构名称
    /// </summary>
    [SugarColumn(ColumnDescription = "发布机构名称", Length = 64)]
    [MaxLength(64)]
    public string? PublicOrgName { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnDescription = "发布时间")]
    public DateTime? PublicTime { get; set; }

    /// <summary>
    /// 撤回时间
    /// </summary>
    [SugarColumn(ColumnDescription = "撤回时间")]
    public DateTime? CancelTime { get; set; }

    /// <summary>
    /// 状态（0草稿 1发布 2撤回 3删除）
    /// </summary>
    [SugarColumn(ColumnDescription = "状态（0草稿 1发布 2撤回 3删除）")]
    public NoticeStatusEnum Status { get; set; }
}