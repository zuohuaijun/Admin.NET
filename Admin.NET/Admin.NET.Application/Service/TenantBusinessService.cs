namespace Admin.NET.Application.Service;

/// <summary>
/// 租户业务服务
/// </summary>
[ApiDescriptionSettings("租户业务服务", Order = 200)]
public class TenantBusinessService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<TenantBusiness> _tenantBusinessRep;

    public TenantBusinessService(SqlSugarRepository<TenantBusiness> businessRep)
    {
        _tenantBusinessRep = businessRep;
    }

    /// <summary>
    /// 增加租户业务数据
    /// </summary>
    /// <returns></returns>
    public async Task<bool> AddBusiness()
    {
        var tenantBusiness = new TenantBusiness() { Name = "zuohuaijun" };
        return await _tenantBusinessRep.InsertAsync(tenantBusiness);
    }

    /// <summary>
    /// 查询租户业务数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<TenantBusiness>> GetBusinessList()
    {
        return await _tenantBusinessRep.GetListAsync();
    }
}