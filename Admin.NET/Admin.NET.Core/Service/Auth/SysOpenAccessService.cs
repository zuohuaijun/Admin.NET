using System.Security.Claims;

namespace Admin.NET.Core.Service;

/// <summary>
/// 开放接口访问服务
/// </summary>
[ApiDescriptionSettings(Order = 510)]
public class SysOpenAccessService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysOpenAccess> _sysOpenAccessRep;
    private readonly SysCacheService _sysCacheService;
    /// <summary>
    /// 开放接口访问服务构造函数
    /// </summary>
    public SysOpenAccessService(SqlSugarRepository<SysOpenAccess> sysOpenAccessRep,
        SysCacheService sysCacheService)
    {
        _sysOpenAccessRep = sysOpenAccessRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取开放接口访问分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取开放接口访问分页列表")]
    public async Task<SqlSugarPagedList<SysOpenAccess>> Page(OpenAccessInput input)
    {
        return await _sysOpenAccessRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccessKey?.Trim()), u => u.AccessKey.Contains(input.AccessKey))
            .OrderBuilder(input)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 根据 Key 获取对象
    /// </summary>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    [HttpGet("getByKey")]
    public Task<SysOpenAccess> GetByKey([FromQuery] string accessKey)
    {
        return Task.FromResult(
            _sysCacheService.GetOrAdd(CacheConst.KeyOpenAccess + accessKey, _ =>
            {
                return _sysOpenAccessRep.AsQueryable()
                    .Includes(u => u.BindUser)
                    .Includes(u => u.BindUser, p => p.SysOrg)
                    .First(u => u.AccessKey == accessKey);
            })
        );
    }

    /// <summary>
    /// Signature 身份验证事件默认实现
    /// </summary>
    [NonAction]
    public static SignatureAuthenticationEvent GetSignatureAuthenticationEventImpl()
    {
        return new SignatureAuthenticationEvent
        {
            OnGetAccessSecret = context =>
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<SysOpenAccessService>>();
                try
                {
                    var openAccessService = context.HttpContext.RequestServices.GetService<SysOpenAccessService>();
                    var openAccess = openAccessService.GetByKey(context.AccessKey).GetAwaiter().GetResult();
                    return Task.FromResult(openAccess == null ? "" : openAccess.AccessSecret);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    return Task.FromResult("");
                }
            },
            OnValidated = context =>
            {
                var openAccessService = context.HttpContext.RequestServices.GetService<SysOpenAccessService>();
                var openAccess = openAccessService.GetByKey(context.AccessKey).GetAwaiter().GetResult();
                var identity = ((ClaimsIdentity)context.Principal!.Identity!);

                identity.AddClaims(new[]
                {
                    new Claim(ClaimConst.UserId, openAccess.BindUser.Id + ""),
                    new Claim(ClaimConst.TenantId, openAccess.BindUser.TenantId + ""),
                    new Claim(ClaimConst.Account, openAccess.BindUser.Account + ""),
                    new Claim(ClaimConst.RealName, openAccess.BindUser.RealName),
                    new Claim(ClaimConst.AccountType, ((int) openAccess.BindUser.AccountType).ToString()),
                    new Claim(ClaimConst.OrgId, openAccess.BindUser.OrgId + ""),
                    new Claim(ClaimConst.OrgName, openAccess.BindUser.SysOrg?.Name + ""),
                    new Claim(ClaimConst.OrgType, openAccess.BindUser.SysOrg?.Type + ""),
                });
                return Task.CompletedTask;
            }
        };
    }
}