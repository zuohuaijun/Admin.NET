// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Plugin.GoView.Service;

/// <summary>
/// 系统登录服务
/// </summary>
[UnifyProvider("GoView")]
[ApiDescriptionSettings(GoViewConst.GroupName, Module = "goview", Name = "sys", Order = 500)]
public class GoViewSysService : IDynamicApiController
{
    private readonly SysAuthService _sysAuthService;
    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SysCacheService _sysCacheService;

    public GoViewSysService(SysAuthService sysAuthService,
        SqlSugarRepository<SysUser> sysUserRep,
        SysCacheService sysCacheService)
    {
        _sysAuthService = sysAuthService;
        _sysUserRep = sysUserRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// GoView 登录
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("GoView 登录")]
    public async Task<GoViewLoginOutput> Login(GoViewLoginInput input)
    {
        _sysCacheService.Set(CommonConst.SysCaptcha, false);

        var loginResult = await _sysAuthService.Login(new LoginInput()
        {
            Account = input.Username,
            Password = input.Password,
        });

        _sysCacheService.Remove(CommonConst.SysCaptcha);

        var sysUser = await _sysUserRep.AsQueryable().Filter(null, true).FirstAsync(u => u.Account.Equals(input.Username));
        return new GoViewLoginOutput()
        {
            Userinfo = new GoViewLoginUserInfo
            {
                Id = sysUser.Id.ToString(),
                Username = sysUser.Account,
                Nickname = sysUser.NickName,
            },
            Token = new GoViewLoginToken
            {
                TokenValue = $"Bearer {loginResult.AccessToken}"
            }
        };
    }

    /// <summary>
    /// GoView 退出
    /// </summary>
    [DisplayName("GoView 退出")]
    public void GetLogout()
    {
        _sysAuthService.Logout();
    }

    /// <summary>
    /// 获取 OSS 上传接口
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetOssInfo")]
    [DisplayName("获取 OSS 上传接口")]
    public Task<GoViewOssUrlOutput> GetOssInfo()
    {
        return Task.FromResult(new GoViewOssUrlOutput { BucketURL = "" });
    }
}