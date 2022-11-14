namespace Admin.NET.Application.Entity;

/// <summary>
/// 租户业务分库表
/// </summary>
[SugarTable("d_tenant_business", "租户业务分库表")]
[TenantBusiness]
public class TenantBusiness : EntityTenant
{
    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "姓名", Length = 32)]
    [Required, MaxLength(32)]
    public virtual string Name { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    [SugarColumn(ColumnDescription = "年龄")]
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnDescription = "出生日期")]
    public DateTime BirthDate { get; set; }
}