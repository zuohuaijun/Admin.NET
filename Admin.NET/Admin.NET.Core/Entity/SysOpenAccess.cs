// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 开放接口身份表
/// </summary>
[SugarTable(null, "开放接口身份表")]
[SysTable]
[SugarIndex("index_{table}_A", nameof(AccessKey), OrderByType.Asc)]
public class SysOpenAccess : EntityBase
{
    /// <summary>
    /// 身份标识
    /// </summary>
    [SugarColumn(ColumnDescription = "身份标识", Length = 128)]
    [Required, MaxLength(128)]
    public virtual string AccessKey { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    [SugarColumn(ColumnDescription = "密钥", Length = 256)]
    [Required, MaxLength(256)]
    public virtual string AccessSecret { get; set; }

    /// <summary>
    /// 绑定租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "绑定租户Id")]
    public long BindTenantId { get; set; }

    /// <summary>
    /// 绑定租户
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [Navigate(NavigateType.OneToOne, nameof(BindTenantId))]
    public SysTenant BindTenant { get; set; }

    /// <summary>
    /// 绑定用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "绑定用户Id")]
    public virtual long BindUserId { get; set; }

    /// <summary>
    /// 绑定用户
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [Navigate(NavigateType.OneToOne, nameof(BindUserId))]
    public SysUser BindUser { get; set; }
}