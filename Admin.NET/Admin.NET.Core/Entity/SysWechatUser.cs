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
/// 系统微信用户表
/// </summary>
[SugarTable(null, "系统微信用户表")]
[SysTable]
public class SysWechatUser : EntityBase
{
    /// <summary>
    /// 系统用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "系统用户Id")]
    public long UserId { get; set; }

    /// <summary>
    /// 系统用户
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public SysUser SysUser { get; set; }

    /// <summary>
    /// 平台类型
    /// </summary>
    [SugarColumn(ColumnDescription = "平台类型")]
    public PlatformTypeEnum PlatformType { get; set; } = PlatformTypeEnum.微信公众号;

    /// <summary>
    /// OpenId
    /// </summary>
    [SugarColumn(ColumnDescription = "OpenId", Length = 64)]
    [Required, MaxLength(64)]
    public virtual string OpenId { get; set; }

    /// <summary>
    /// 会话密钥
    /// </summary>
    [SugarColumn(ColumnDescription = "会话密钥", Length = 256)]
    [MaxLength(256)]
    public string? SessionKey { get; set; }

    /// <summary>
    /// UnionId
    /// </summary>
    [SugarColumn(ColumnDescription = "UnionId", Length = 64)]
    [MaxLength(64)]
    public string? UnionId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 64)]
    [MaxLength(64)]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnDescription = "头像", Length = 256)]
    [MaxLength(256)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [SugarColumn(ColumnDescription = "手机号码", Length = 16)]
    [MaxLength(16)]
    public string? Mobile { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [SugarColumn(ColumnDescription = "性别")]
    public int? Sex { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    [SugarColumn(ColumnDescription = "语言", Length = 64)]
    [MaxLength(64)]
    public string? Language { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    [SugarColumn(ColumnDescription = "城市", Length = 64)]
    [MaxLength(64)]
    public string? City { get; set; }

    /// <summary>
    /// 省
    /// </summary>
    [SugarColumn(ColumnDescription = "省", Length = 64)]
    [MaxLength(64)]
    public string? Province { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    [SugarColumn(ColumnDescription = "国家", Length = 64)]
    [MaxLength(64)]
    public string? Country { get; set; }

    /// <summary>
    /// AccessToken
    /// </summary>
    [SugarColumn(ColumnDescription = "AccessToken", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? AccessToken { get; set; }

    /// <summary>
    /// RefreshToken
    /// </summary>
    [SugarColumn(ColumnDescription = "RefreshToken", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    [SugarColumn(ColumnDescription = "ExpiresIn")]
    public int? ExpiresIn { get; set; }

    /// <summary>
    /// 用户授权的作用域，使用逗号分隔
    /// </summary>
    [SugarColumn(ColumnDescription = "授权作用域", Length = 64)]
    [MaxLength(64)]
    public string? Scope { get; set; }
}