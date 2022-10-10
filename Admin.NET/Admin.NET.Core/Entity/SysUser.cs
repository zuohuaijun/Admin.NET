namespace Admin.NET.Core;

/// <summary>
/// 系统用户表
/// </summary>
[SugarTable("sys_user", "系统用户表")]
public class SysUser : EntityTenant
{
    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(ColumnDescription = "账号名称", Length = 32)]
    [Required, MaxLength(32)]
    public virtual string UserName { get; set; }

    /// <summary>
    /// 账号密码（MD5加密）
    /// </summary>
    [SugarColumn(ColumnDescription = "账号密码", Length = 64)]
    [Required, MaxLength(64)]
    [System.Text.Json.Serialization.JsonIgnore]
    [JsonIgnore]
    public virtual string Password { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(ColumnDescription = "昵称", Length = 32)]
    [MaxLength(32)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(ColumnDescription = "头像", Length = 256)]
    [MaxLength(256)]
    public string Avatar { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnDescription = "出生日期")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 性别-男_1、女_2
    /// </summary>
    [SugarColumn(ColumnDescription = "性别")]
    public GenderEnum Sex { get; set; } = GenderEnum.Male;

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnDescription = "邮箱", Length = 64)]
    [MaxLength(64)]
    public string Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [SugarColumn(ColumnDescription = "手机号码", Length = 16)]
    [MaxLength(16)]
    public virtual string Phone { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "真实姓名", Length = 32)]
    [MaxLength(32)]
    public virtual string RealName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [SugarColumn(ColumnDescription = "身份证号", Length = 32)]
    [MaxLength(32)]
    public virtual string IdCard { get; set; }

    /// <summary>
    /// 个性签名
    /// </summary>
    [SugarColumn(ColumnDescription = "个性签名", Length = 64)]
    [MaxLength(64)]
    public string Signature { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [SugarColumn(ColumnDescription = "个人简介", Length = 512)]
    [MaxLength(512)]
    public string Introduction { get; set; }

    /// <summary>
    /// 账号类型-超级管理员_1、管理员_2、普通_3
    /// </summary>
    [SugarColumn(ColumnDescription = "账号类型")]
    public UserTypeEnum UserType { get; set; } = UserTypeEnum.None;

    /// <summary>
    /// 机构Id
    /// </summary>
    [SugarColumn(ColumnDescription = "机构Id")]
    public long OrgId { get; set; }

    /// <summary>
    /// 机构
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(OrgId))]
    public SysOrg SysOrg { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职位Id")]
    public long PosId { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(PosId))]
    public SysPos SysPos { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [SugarColumn(ColumnDescription = "工号", Length = 32)]
    [MaxLength(32)]
    public string JobNum { get; set; }

    /// <summary>
    /// 岗位状态
    /// </summary>
    [SugarColumn(ColumnDescription = "岗位状态")]
    public JobStatusEnum JobStatus { get; set; } = JobStatusEnum.On;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Order { get; set; } = 100;

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 128)]
    [MaxLength(128)]
    public string Remark { get; set; }
}