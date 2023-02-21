namespace Admin.NET.Core;

/// <summary>
/// 假删除接口过滤器
/// </summary>
internal interface IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    bool IsDelete { get; set; }
}

/// <summary>
/// 租户Id接口过滤器
/// </summary>
internal interface ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    long? TenantId { get; set; }
}

/// <summary>
/// 机构Id接口过滤器
/// </summary>
internal interface IOrgIdFilter
{
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    long? CreateOrgId { get; set; }
}