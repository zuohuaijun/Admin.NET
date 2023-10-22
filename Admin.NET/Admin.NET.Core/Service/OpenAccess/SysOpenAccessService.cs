// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System.Security.Claims;

namespace Admin.NET.Core.Service;

/// <summary>
/// 开放接口身份服务
/// </summary>
[ApiDescriptionSettings(Order = 244)]
public class SysOpenAccessService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysOpenAccess> _sysOpenAccessRep;
    private readonly SysCacheService _sysCacheService;

    /// <summary>
    /// 开放接口身份服务构造函数
    /// </summary>
    public SysOpenAccessService(SqlSugarRepository<SysOpenAccess> sysOpenAccessRep,
        SysCacheService sysCacheService)
    {
        _sysOpenAccessRep = sysOpenAccessRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取开放接口身份分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取开放接口身份分页列表")]
    public async Task<SqlSugarPagedList<OpenAccessOutput>> Page(OpenAccessInput input)
    {
        return await _sysOpenAccessRep.AsQueryable()
            .LeftJoin<SysUser>((u, a) => u.BindUserId == a.Id)
            .LeftJoin<SysTenant>((u, a, b) => u.BindTenantId == b.Id)
            .LeftJoin<SysOrg>((u, a, b, c) => b.OrgId == c.Id)
            .WhereIF(!string.IsNullOrWhiteSpace(input.AccessKey?.Trim()), (u, a, b, c) => u.AccessKey.Contains(input.AccessKey))
            .Select((u, a, b, c) => new OpenAccessOutput
            {
                BindUserAccount = a.Account,
                BindTenantName = c.Name,
            }, true)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加开放接口身份
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加开放接口身份")]
    public async Task AddOpenAccess(AddOpenAccessInput input)
    {
        if (await _sysOpenAccessRep.AsQueryable().AnyAsync(u => u.AccessKey == input.AccessKey && u.Id != input.Id))
            throw Oops.Oh(ErrorCodeEnum.O1000);

        var openAccess = input.Adapt<SysOpenAccess>();
        await _sysOpenAccessRep.InsertAsync(openAccess);
    }

    /// <summary>
    /// 更新开放接口身份
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新开放接口身份")]
    public async Task UpdateOpenAccess(UpdateOpenAccessInput input)
    {
        if (await _sysOpenAccessRep.AsQueryable().AnyAsync(u => u.AccessKey == input.AccessKey && u.Id != input.Id))
            throw Oops.Oh(ErrorCodeEnum.O1000);

        var openAccess = input.Adapt<SysOpenAccess>();
        _sysCacheService.Remove(CacheConst.KeyOpenAccess + openAccess.AccessKey);

        await _sysOpenAccessRep.UpdateAsync(openAccess);
    }

    /// <summary>
    /// 删除开放接口身份
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除开放接口身份")]
    public async Task DeleteOpenAccess(DeleteOpenAccessInput input)
    {
        var openAccess = await _sysOpenAccessRep.GetFirstAsync(u => u.Id == input.Id);
        if (openAccess != null)
            _sysCacheService.Remove(CacheConst.KeyOpenAccess + openAccess.AccessKey);

        await _sysOpenAccessRep.DeleteAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 创建密钥
    /// </summary>
    /// <returns></returns>
    [DisplayName("创建密钥")]
    public Task<string> CreateSecret()
    {
        return Task.FromResult(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..^2]);
    }

    /// <summary>
    /// 根据 Key 获取对象
    /// </summary>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    [NonAction]
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
                    new Claim(ClaimConst.UserId, openAccess.BindUserId + ""),
                    new Claim(ClaimConst.TenantId, openAccess.BindTenantId + ""),
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