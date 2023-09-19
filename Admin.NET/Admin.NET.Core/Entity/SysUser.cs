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
/// 系统用户表
/// </summary>
[SugarTable(null, "系统用户表")]
[SysTable]
public class SysUser : EntityTenant
{
    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号", Length = 32)]
    [Required, MaxLength(32)]
    public virtual string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(ColumnDescription = "密码", Length = 512)]
    [MaxLength(512)]
    [System.Text.Json.Serialization.JsonIgnore]
    [JsonIgnore]
    public virtual string Password { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 32)]
    [MaxLength(32)]
    public virtual string RealName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 32)]
    [MaxLength(32)]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnDescription = "头像", Length = 512)]
    [MaxLength(512)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    [SugarColumn(ColumnDescription = "性别")]
    public GenderEnum Sex { get; set; } = GenderEnum.Male;

    /// <summary>
    /// 年龄
    /// </summary>
    [SugarColumn(ColumnDescription = "年龄")]
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnDescription = "出生日期")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 民族
    /// </summary>
    [SugarColumn(ColumnDescription = "民族", Length = 32)]
    [MaxLength(32)]
    public string? Nation { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [SugarColumn(ColumnDescription = "手机号码", Length = 16)]
    [MaxLength(16)]
    public string? Phone { get; set; }

    /// <summary>
    /// 证件类型
    /// </summary>
    [SugarColumn(ColumnDescription = "证件类型")]
    public CardTypeEnum CardType { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [SugarColumn(ColumnDescription = "身份证号", Length = 32)]
    [MaxLength(32)]
    public string? IdCardNum { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "邮箱", Length = 64)]
    [MaxLength(64)]
    public string? Email { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [SugarColumn(ColumnDescription = "地址", Length = 256)]
    [MaxLength(256)]
    public string? Address { get; set; }

    /// <summary>
    /// 文化程度
    /// </summary>
    [SugarColumn(ColumnDescription = "文化程度")]
    public CultureLevelEnum CultureLevel { get; set; }

    /// <summary>
    /// 政治面貌
    /// </summary>
    [SugarColumn(ColumnDescription = "政治面貌", Length = 16)]
    [MaxLength(16)]
    public string? PoliticalOutlook { get; set; }

    /// <summary>
    /// 毕业院校
    /// </summary>COLLEGE
    [SugarColumn(ColumnDescription = "毕业院校", Length = 128)]
    [MaxLength(128)]
    public string? College { get; set; }

    /// <summary>
    /// 办公电话
    /// </summary>
    [SugarColumn(ColumnDescription = "办公电话", Length = 16)]
    [MaxLength(16)]
    public string? OfficePhone { get; set; }

    /// <summary>
    /// 紧急联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系人", Length = 32)]
    [MaxLength(32)]
    public string? EmergencyContact { get; set; }

    /// <summary>
    /// 紧急联系人电话
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系人电话", Length = 16)]
    [MaxLength(16)]
    public string? EmergencyPhone { get; set; }

    /// <summary>
    /// 紧急联系人地址
    /// </summary>
    [SugarColumn(ColumnDescription = "紧急联系人地址", Length = 256)]
    [MaxLength(256)]
    public string? EmergencyAddress { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [SugarColumn(ColumnDescription = "个人简介", Length = 512)]
    [MaxLength(512)]
    public string? Introduction { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int OrderNo { get; set; } = 100;

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 256)]
    [MaxLength(256)]
    public string? Remark { get; set; }

    /// <summary>
    /// 账号类型
    /// </summary>
    [SugarColumn(ColumnDescription = "账号类型")]
    public AccountTypeEnum AccountType { get; set; } = AccountTypeEnum.NormalUser;

    /// <summary>
    /// 直属机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "直属机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 直属机构
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(OrgId))]
    public SysOrg SysOrg { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    [SugarColumn(ColumnDescription = "直属主管Id")]
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// 直属主管
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ManagerUserId))]
    public SysUser ManagerUser { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职位Id")]
    public long PosId { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(PosId))]
    public SysPos SysPos { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [SugarColumn(ColumnDescription = "工号", Length = 32)]
    [MaxLength(32)]
    public string? JobNum { get; set; }

    /// <summary>
    /// 职级
    /// </summary>
    [SugarColumn(ColumnDescription = "职级", Length = 32)]
    [MaxLength(32)]
    public string? PosLevel { get; set; }

    /// <summary>
    /// 职称
    /// </summary>
    [SugarColumn(ColumnDescription = "职称", Length = 32)]
    [MaxLength(32)]
    public string? PosTitle { get; set; }

    /// <summary>
    /// 擅长领域
    /// </summary>
    [SugarColumn(ColumnDescription = "擅长领域", Length = 32)]
    [MaxLength(32)]
    public string? Expertise { get; set; }

    /// <summary>
    /// 办公区域
    /// </summary>
    [SugarColumn(ColumnDescription = "办公区域", Length = 32)]
    [MaxLength(32)]
    public string? OfficeZone { get; set; }

    /// <summary>
    /// 办公室
    /// </summary>
    [SugarColumn(ColumnDescription = "办公室", Length = 32)]
    [MaxLength(32)]
    public string? Office { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    [SugarColumn(ColumnDescription = "入职日期")]
    public DateTime? JoinDate { get; set; }

    /// <summary>
    /// 最新登录Ip
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录Ip", Length = 256)]
    [MaxLength(256)]
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 最新登录地点
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录地点", Length = 128)]
    [MaxLength(128)]
    public string? LastLoginAddress { get; set; }

    /// <summary>
    /// 最新登录时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录时间")]
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最新登录设备
    /// </summary>
    [SugarColumn(ColumnDescription = "最新登录设备", Length = 128)]
    [MaxLength(128)]
    public string? LastLoginDevice { get; set; }

    /// <summary>
    /// 电子签名
    /// </summary>
    [SugarColumn(ColumnDescription = "电子签名", Length = 512)]
    [MaxLength(512)]
    public string? Signature { get; set; }
}